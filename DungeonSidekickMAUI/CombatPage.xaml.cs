using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;

public partial class CombatPage : ContentPage
{
    private Inventory inv;
    private Monster currentMonster;
    private int selectedWeaponId;
	public CombatPage()
	{
		InitializeComponent();
        PullWeaponInventory();
    }
	void PullWeaponInventory()
	{
        Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
        Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
        Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];

        inv = new Inventory();
        inv.PullItems();
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        foreach (var weapon in inv.Weapons)
        {
            string query = "SELECT name FROM dbo.Weapon" +
            " WHERE WeaponID = @Id;";
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

                            // grabs ID from weapon list
                            cmd.Parameters.AddWithValue("@Id", weapon[0]);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    HorizontalStackLayout layout = new HorizontalStackLayout();
                                    Label weaponLabel = new Label();
                                    weaponLabel.TextColor = (Color)fontColor;

                                    // grabs the name from the DB and quantity from weapon list
                                    string name = reader.GetString(0);
                                    weaponLabel.Text = name + " x" + weapon[1];
                                    layout.Add(weaponLabel);

                                    // Button that selects the item to be used in combat
                                    Button select = new Button
                                    {
                                        CommandParameter = weapon[0],
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
    private async void SelectButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int id)
        {
            selectedWeaponId = id;
            await DisplayAlert("Selected Weapon", "Successfully selected the item for combat.", "Ok");
        }
    }
    private void SelectMonster(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SelectMonster());
    }
}