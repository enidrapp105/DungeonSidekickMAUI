using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;

namespace DungeonSidekickMAUI;

public partial class InventoryPage : ContentPage
{
    Inventory inv;
    int characterid;
    Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
    Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
    Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
    Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];
    Color AccentColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["AccentColor"];
    Color AccessoryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["AccessoryColor"];
    ImportedCharacterSheet currentcharacterSheet = ImportedCharacterSheet.Instance;
    public InventoryPage()
	{
		InitializeComponent();
        inv = new Inventory();
        inv.PullItems();

        // nav bar setup
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, false);
        var customNavBar = cmd.CreateCustomNavigationBar();
        NavigationBar.Children.Add(customNavBar);

        PullWeapons();
        PullArmor();
        PullGear();
    }

    // Anthony
    private void AddItem(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddToInventory());
    }
    private async void RemoveButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is UserItem userItem)
        {
            int eTypeId = userItem.eTypeId;
            int id = userItem.Id;
            inv.RemoveEquipment(userItem.Id, userItem.eTypeId);
            var page = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new InventoryPage());
            Navigation.RemovePage(page);
        }
    }


    /*
     * Function: EquipButton
     * Author: Thomas Hewitt
     * Purpose: Handles equipping armor when the Equip button is clicked.
     * Last Modified: 5/5/2024 11:51pm
     */
    private async void EquipButton(object? sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is UserItem userItem)
        {
            int eTypeId = userItem.eTypeId;
            int id = userItem.Id;
            currentcharacterSheet.EquipItem(id, eTypeId);
            await Navigation.PushAsync(new InventoryPage()); // Only using await here because it complains otherwise. It's much faster without it.
            Navigation.RemovePage(this); // Getting rid of the old page so the back button has fewer issues.
        }
    }

    private void PullGear()
    {
        Connection connection = Connection.connectionSingleton;
        foreach (var gear in inv.Gear)
        {
            string query = "SELECT name FROM dbo.Gear" +
            " WHERE GearId = @Id;";
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

                            // grabs ID from items list
                            cmd.Parameters.AddWithValue("@Id", gear[0]);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserItem temp = new UserItem();
                                    HorizontalStackLayout layout = new HorizontalStackLayout();
                                    Label gearLabel = new Label();
                                    gearLabel.TextColor = (Color)fontColor;
                                    

                                    // grabs the name from the DB and quantity from items list
                                    string name = reader.GetString(0);
                                    gearLabel.Text = name + " x" + gear[1];
                                    
                                    temp.Name = name;
                                    temp.Id = gear[0];
                                    temp.eTypeId = 2;

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

                                    InvStack.Add(layout);
                                    layout.Add(gearLabel);
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
    private void PullArmor()
    {
        Connection connection = Connection.connectionSingleton;
        foreach (var armor in inv.Equipment)
        {
            string query = "SELECT name FROM dbo.Armor" +
            " WHERE ArmorID = @Id;";
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

                            // grabs ID from equipment list
                            cmd.Parameters.AddWithValue("@Id", armor[0]);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserItem temp = new UserItem();
                                    HorizontalStackLayout layout = new HorizontalStackLayout();
                                    Label armorLabel = new Label();
                                    armorLabel.TextColor = (Color)fontColor;

                                    // grabs the name from the DB and quantity from equipment list
                                    string name = reader.GetString(0);
                                    armorLabel.Text = name + " x" + armor[1];
                                    
                                    temp.Name = name;
                                    temp.Id = armor[0];
                                    temp.eTypeId = 1;

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
                                    

                                    // Only shows if you aren't wearing this armor. Only works on page refresh, though.
                                    if (temp.Id != currentcharacterSheet.c_EEquippedID)
                                    {
                                        // Button to equip armor.
                                        Button equip = new Button
                                        {
                                            TextColor = fontColor,
                                            Text = "Equip",
                                            BackgroundColor = AccentColor,
                                            CommandParameter = temp,
                                            HorizontalOptions = LayoutOptions.End,
                                            Margin = new Thickness(10, 0, 10, 10)
                                        };
                                        equip.Clicked += EquipButton;
                                        layout.Add(equip);
                                    }

                                    layout.Add(armorLabel);
                                    InvStack.Add(layout);
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
    private void PullWeapons()
    {
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
                                    UserItem temp = new UserItem();
                                    HorizontalStackLayout layout = new HorizontalStackLayout();
                                    Label weaponLabel = new Label();
                                    weaponLabel.TextColor = (Color)fontColor;

                                    // grabs the name from the DB and quantity from weapon list
                                    string name = reader.GetString(0);
                                    weaponLabel.Text = name + " x" + weapon[1];
                                    temp.Name = name;
                                    temp.Id = weapon[0];
                                    temp.eTypeId = 0;
                                    

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
                                    InvStack.Add(layout);
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
}