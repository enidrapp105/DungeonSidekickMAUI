using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace DungeonSidekickMAUI;

public partial class SpellCombatPage : ContentPage
{
    private Spellpool spells;
    private Monster currentMonster;
    private int selectedWeaponId;
    public SpellCombatPage()
    {
        InitializeComponent();
        PullSpellsPool();
    }
    /*
     * Function: PullSpellsPool
     * Author: Brendon Williams
     * Purpose: Works like weapon pool, displays all spells to user
     * Last Modified: 04/15/2024 By Author
     */
    void PullSpellsPool()
    {
        Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
        Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
        Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];

        spells = new Spellpool();
        spells.PullSpells();
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        foreach (var spell in spells.Spells)
        {
            string query = "SELECT name FROM dbo.Spells" +
            " WHERE SpellID = @Id;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;

                            // grabs ID from spell pool list
                            cmd.Parameters.AddWithValue("@Id", spell);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    HorizontalStackLayout layout = new HorizontalStackLayout();
                                    Label weaponLabel = new Label();
                                    weaponLabel.TextColor = (Color)fontColor;

                                    // should hopefully grab name of spell from spell pool
                                    string name = reader.GetString(0);
                                    weaponLabel.Text = name;
                                    layout.Add(weaponLabel);

                                    // Button that selects the item to be used in combat
                                    Button select = new Button
                                    {
                                        CommandParameter = spell,
                                        TextColor = fontColor,
                                        Text = "Select",
                                        BackgroundColor = TrinaryColor,
                                    };
                                    select.Clicked += SelectButton;
                                    layout.Add(select);
                                    CombatStack.Add(layout);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                DisplayAlert("Error!", eSql.Message, "OK");
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }
    }

    /*
     * Function: SelectButton
     * Author: Brendon Williams
     * Purpose: Selects spell for user. Currently doesn't work
     * Last Modified: 04/15/2024 By Author
     */
    private async void SelectButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int id)
        {
            selectedWeaponId = id;
            Preferences.Default.Set("SelectedWeapon", id);
            await DisplayAlert("Selected Weapon", "Successfully selected the item for combat.", "Ok");
        }
    }

    /*
     * Function: SelectMonster
     * Author: Brendon Williams
     * Purpose: Moves to selecting monsters to fight
     * Last Modified: 04/15/2024 By Author
     */
    private void SelectMonster(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SelectMonster());
    }

    /*
     * Function: AddSpells
     * Author: Anthony Rielly
     * Purpose: Move to the add to spellpool page
     * Last Modified: 04/18/2024 By Author
     */
    private void AddSpells(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddToSpellpool());
    }
}