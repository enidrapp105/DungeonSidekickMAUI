using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;

public partial class CombatPage : ContentPage
{
    private Inventory inv;
    private Monster currentMonster;
    private bool selected;
	public CombatPage()
	{
		InitializeComponent();
        selected = false;

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, true);
        ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());

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
        Connection connection = Connection.connectionSingleton;
        foreach (var weapon in inv.Weapons)
        {
            string query = "SELECT name FROM dbo.Weapon" +
            " WHERE WeaponID = @Id;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connection.connectionString))
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
                                    

                                    // Button that selects the item to be used in combat
                                    Button select = new Button
                                    {
                                        CommandParameter = weapon[0],
                                        TextColor = fontColor,
                                        Text = "Select",
                                        BackgroundColor = TrinaryColor,
                                        Margin = new Thickness(10,0,10,10)
                                    };
                                    select.Clicked += SelectButton;
                                    layout.Add(select);
                                    layout.Add(weaponLabel);
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
            ImportedCharacterSheet character = ImportedCharacterSheet.Instance;
            character.EquipItem(id, 0); // Equipping Weapon.
            selected = true;
            await DisplayAlert("Selected Weapon", "Successfully selected the item for combat.", "Ok");
        }
    }
    private void SelectMonster(object sender, EventArgs e)
    {
        if (selected)
        {
            Navigation.PushAsync(new SelectMonster());
        }
        else
        {
            DisplayAlert("No Weapon Selected", "Please select a weapon before proceeding. If you don't see any weapons, you can add one in the inventory page.", "Ok");
        }
    }
}