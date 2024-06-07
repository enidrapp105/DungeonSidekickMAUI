using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
namespace DungeonSidekickMAUI;

public partial class SpellpoolPage : ContentPage
{
    private Spellpool spells;
    public SpellpoolPage()
	{
		InitializeComponent();

        // nav bar setup
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, false);
        var customNavBar = cmd.CreateCustomNavigationBar();
        NavigationBar.Children.Add(customNavBar);

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
        Color AccentColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["AccentColor"];
        Color AccessoryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["AccessoryColor"];

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

                                    Spells temp = new Spells();
                                    temp.Id = spell;
                                    temp.Name = name;

                                    // Button that removes the item from the DB
                                    Button delete = new Button
                                    {
                                        TextColor = fontColor,
                                        Text = "Remove",
                                        BackgroundColor = AccentColor,
                                        CommandParameter = temp,
                                        HorizontalOptions = LayoutOptions.End,
                                        Margin = new Thickness(10, 0, 10, 10)
                                    };
                                    delete.Clicked += RemoveButton;
                                    layout.Add(delete);
                                    layout.Add(weaponLabel);

                                    SpellpoolStack.Add(layout);
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

    private async void RemoveButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Spells spell)
        {
            spells.RemoveSpell(spell.Id);
            var page = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new SpellpoolPage());
            Navigation.RemovePage(page);
        }
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