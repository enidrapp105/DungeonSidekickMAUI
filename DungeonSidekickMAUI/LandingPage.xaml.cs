using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;
public partial class LandingPage : ContentPage
{
    ImportedCharacterSheet currentcharacterSheet = ImportedCharacterSheet.Instance;
    ImportedCharacterSheet currentcharacterSheet2 = ImportedCharacterSheet.Load();
    DiceRoll diceroller;
    Inventory inv;
    List<string> statusnames;
    List<string> statusdescriptions;
    List<string> exhaustiondescriptions;
    HashSet<string> existingstatuses;
    // Allows us to use dynamic colors when creating labels, buttons, etc in this class
    Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
    Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
    Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
    Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];

    public LandingPage()
	{

        InitializeComponent();

        LoadCharacterSheetPage(currentcharacterSheet2);

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands nav = new NavigationCommands();
        Microsoft.Maui.Controls.NavigationPage.SetHasNavigationBar(this, true);
        ((Microsoft.Maui.Controls.NavigationPage)Microsoft.Maui.Controls.Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        Microsoft.Maui.Controls.NavigationPage.SetTitleView(this, nav.CreateCustomNavigationBar());

        diceroller = new DiceRoll();
        //if (currentcharacterSheet != null ) 
        //{
        //    LoadCharacterSheetPage(currentcharacterSheet);
        //}
        //else
        //{
        //    DisplayAlert("Your character sheet didn't convert correctly", "Let's retry making one", "Ok"); //In case the character sheet breaks
        //    Navigation.PushAsync(new MainPage()); //at some point during the programming process
        //}

        inv = new Inventory(); // TEMP PLACEHOLDER 1
        inv.PullItems();

        Connection connection = Connection.connectionSingleton;
        statusnames = new List<string>();
        statusdescriptions = new List<string>();
        existingstatuses = new HashSet<string>();
        exhaustiondescriptions = new List<string>();

        using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
        {
            string sqlQuery = "SELECT name, description FROM Conditions;";

            SqlCommand command = new SqlCommand(sqlQuery, conn);


            conn.Open();

            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    string description = reader["description"].ToString();

                    statusnames.Add(name);
                    statusdescriptions.Add(description);
                }

                reader.Close();
        }
        using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
        {
            string sqlQuery = "SELECT EffectDescription FROM dbo.ExhaustionEffects;";

            SqlCommand command = new SqlCommand(sqlQuery, conn);

            conn.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string exhaustiondescription = reader["EffectDescription"].ToString();
                exhaustiondescriptions.Add(exhaustiondescription);
            }

            reader.Close();
        }
        StatusEffectPicker.ItemsSource = statusnames;
        foreach (var weapon in inv.Weapons)
        {
            string query = "SELECT name FROM dbo.Weapon" +
            " WHERE WeaponID = @Id;";
            try
            {
                using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
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
                                    layout.Add(weaponLabel);

                                    // Button that removes the item from the DB
                                    Button delete = new Button
                                    {
                                        TextColor = fontColor,
                                        Text = "Remove",
                                        BackgroundColor = TrinaryColor,
                                        CommandParameter = temp
                                    };
                                    delete.Clicked += RemoveButton;
                                    layout.Add(delete);
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

        foreach (var armor in inv.Equipment)
        {
            string query = "SELECT name FROM dbo.Armor" +
            " WHERE ArmorID = @Id;";
            try
            {
                using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
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
                                    layout.Add(armorLabel);
                                    temp.Name = name;
                                    temp.Id = armor[0];
                                    temp.eTypeId = 1;

                                    // Button that removes the item from the DB
                                    Button delete = new Button
                                    {
                                        TextColor = fontColor,
                                        Text = "Remove",
                                        BackgroundColor = TrinaryColor,
                                        CommandParameter = temp
                                    };
                                    
                                    delete.Clicked += RemoveButton;
                                    layout.Add(delete);
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

        foreach (var gear in inv.Items)
        {
            string query = "SELECT name FROM dbo.Gear" +
            " WHERE GearId = @Id;";
            try
            {
                using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
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
                                    layout.Add(gearLabel);
                                    temp.Name = name;
                                    temp.Id = gear[0];
                                    temp.eTypeId = 2;

                                    // Button that removes the item from the DB
                                    Button delete = new Button
                                    {
                                        TextColor = fontColor,
                                        Text = "Remove",
                                        BackgroundColor = TrinaryColor,
                                        CommandParameter = temp

                                    };
                                    delete.Clicked += RemoveButton;
                                    layout.Add(delete);

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

    private async void AddEffectButtonClicked(object sender, EventArgs e)
    {
        if (StatusEffectPicker.SelectedItem == null)
        {
            await DisplayAlert("No Effect Selected", "Please select an effect", "Ok");
            return;
        }

        string selectedEffect = StatusEffectPicker.SelectedItem.ToString();
        int index = statusnames.IndexOf(selectedEffect);

        if (existingstatuses.Contains(selectedEffect))
        {
            await DisplayAlert("Effect already present", "Please select a different effect", "Ok");
            return;
        }

        if (selectedEffect == "Exhaustion")
        {
            // Create an ExhaustionView with the descriptions
            ExhaustionView exhaustionView = new ExhaustionView(exhaustiondescriptions)
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 5, 0, 0)
            };

            Button removeButton = new Button
            {
                Text = "Remove",
                BackgroundColor = SecondaryColor,
                TextColor = fontColor,
                Margin = new Thickness(0, 5, 0, 0)
            };
            removeButton.Clicked += (s, args) =>
            {
                statusstack.Children.Remove(exhaustionView);
                statusstack.Children.Remove(removeButton);
                existingstatuses.Remove(selectedEffect);
            };

            statusstack.Children.Add(exhaustionView);
            statusstack.Children.Add(removeButton);
            existingstatuses.Add(selectedEffect);
        }
        else
        {
            // Handle other status effects
            if (index >= 0 && index < statusdescriptions.Count)
            {
                string description = statusdescriptions[index];
                Label effectLabel = new Label
                {
                    Text = $"{selectedEffect}: {description}",
                    TextColor = fontColor,
                    BackgroundColor = PrimaryColor,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                Button removeButton = new Button
                {
                    Text = "Remove",
                    BackgroundColor = SecondaryColor,
                    TextColor = fontColor,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                removeButton.Clicked += (s, args) =>
                {
                    statusstack.Children.Remove(effectLabel);
                    statusstack.Children.Remove(removeButton);
                    existingstatuses.Remove(selectedEffect);
                };

                statusstack.Children.Add(effectLabel);
                statusstack.Children.Add(removeButton);
                existingstatuses.Add(selectedEffect);
            }
        }
    }

    private async void RemoveButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is UserItem userItem)
        {
            int eTypeId = userItem.eTypeId;
            int id = userItem.Id;
            inv.RemoveEquipment(userItem.Id, userItem.eTypeId);
            var page = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new LandingPage());
            Navigation.RemovePage(page);
        }
    }
    /*
     * Function: CalcStatMod
     * Author: Brendon Williams
     * Purpose: Does the math to calculate a stat's modifier
     * Last Modified: 2/10/2024 12:04pm by Author
     */
    private int CalcStatMod(int stat)
    {
        int mod = stat - 10;
        double truemod = Math.Floor(mod / 2.0); //Double.Floor kind of sucks. Math.Floor is the only way I could get correct negative rounding
        return (int)truemod;
    }

    /*
     * Function: LoadCharacterSheetPage
     * Author: Brendon Williams
     * Purpose: Helper function that calculates the stat modifiers and then saves them as a global variable for later use
     * Last Modified: 2/24/2024 by Author
     */
    private void LoadCharacterSheetPage(ImportedCharacterSheet currentcharacterSheet)
    {

        if(currentcharacterSheet.c_Name != null)
            User_Disp.Text = "Welcome " + currentcharacterSheet.c_Name;
        
        int StrMod = CalcStatMod(currentcharacterSheet.c_Strength);
        int DexMod = CalcStatMod(currentcharacterSheet.c_Dexterity);
        int ConstMod = CalcStatMod(currentcharacterSheet.c_Constitution);
        int IntMod = CalcStatMod(currentcharacterSheet.c_Intelligence);
        int WisMod = CalcStatMod(currentcharacterSheet.c_Wisdom);
        int CharMod = CalcStatMod(currentcharacterSheet.c_Charisma);

        Preferences.Default.Set("StrMod",StrMod);//For each stat, CalcStatMod calculates the modifier based on
        lblStr_Mod.Text = StrMod.ToString(); //the stat that gets passed.

        Preferences.Default.Set("DexMod", DexMod);
        lblDex_Mod.Text = DexMod.ToString();

        Preferences.Default.Set("ConMod", ConstMod);//Using preferences to save the various mods
        lblConst_Mod.Text = ConstMod.ToString(); //when they are calculated

        Preferences.Default.Set("IntMod", IntMod); 
        lblInt_Mod.Text = IntMod.ToString();

        Preferences.Default.Set("WisMod", WisMod);
        lblWis_Mod.Text = WisMod.ToString();

        Preferences.Default.Set("ChaMod", CharMod);
        lblChar_Mod.Text = CharMod.ToString();
    }

    // Anthony
    private void AddItem(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddToInventory());
    }

    private void NavigateToCombat(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CombatSelector());
    }
    /*
     * Function: RollButtonClicked
     * Author: Kenny Rapp
     * Purpose: to call the ParseAndRoll function and set its text to the rolled sum
     * last Modified: 2/4/2024 6:21pm By Kenny Rapp
     */
    //private void RollButtonClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        rollbutton.Text = diceroller.Roll(inputentry.Text).ToString();
    //    }
    //    catch (Exception ex) 
    //    {
    //        DisplayAlert("invalid input(s)", "Please fix your input string", "OK");
    //    }
    //}
}
