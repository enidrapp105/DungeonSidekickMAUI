using System;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;
public partial class LandingPage : ContentPage
{
    CharacterSheet currentcharacterSheet = CharacterSheet.Instance;
    DiceRoll diceroller;
    Inventory inv;
    List<string> statusnames;
    List<string> statusdescriptions;
    List<string> exhaustiondescriptions;
    HashSet<string> existingstatuses;
    int characterid;
    // Allows us to use dynamic colors when creating labels, buttons, etc in this class
    Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
    Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
    Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
    Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];

    public LandingPage()
	{

        InitializeComponent();

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands nav = new NavigationCommands();
        Microsoft.Maui.Controls.NavigationPage.SetHasNavigationBar(this, true);
        ((Microsoft.Maui.Controls.NavigationPage)Microsoft.Maui.Controls.Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        Microsoft.Maui.Controls.NavigationPage.SetTitleView(this, nav.CreateCustomNavigationBar());

        diceroller = new DiceRoll();
        if (currentcharacterSheet != null ) 
        {
            LoadCharacterSheetPage(currentcharacterSheet);
        }
        else
        {
            DisplayAlert("Your character sheet didn't convert correctly", "Let's retry making one", "Ok"); //In case the character sheet breaks
            Navigation.PushAsync(new MainPage()); //at some point during the programming process
        }

        inv = new Inventory(); // TEMP PLACEHOLDER 1
        inv.PullItems();

        Connection connection = Connection.connectionSingleton;
        statusnames = new List<string>();
        statusdescriptions = new List<string>();
        existingstatuses = new HashSet<string>();
        exhaustiondescriptions = new List<string>();
        List<int> characterconditionids = new List<int>();
        List<string> characterconditions = new List<string>();


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
            sqlQuery = "SELECT CharacterID FROM CharacterSheet WHERE CharacterName = @CharacterName;";
            command = new SqlCommand(sqlQuery, conn);
            command.Parameters.AddWithValue("@CharacterName", currentcharacterSheet.charactername);
            if (conn.State == System.Data.ConnectionState.Open)
            {
                // No need to create another SqlCommand here
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    characterid = Convert.ToInt32(result);
                }
            }

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
            sqlQuery = "SElECT ConditionID FROM dbo.CharacterConditions WHERE CharacterID = @characterid;";
            command = new SqlCommand(sqlQuery, conn);
            command.Parameters.AddWithValue("@characterid", characterid);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                int ConditionId = (int)reader["ConditionID"];
                characterconditionids.Add(ConditionId);
            }
            reader.Close();
            foreach (int conditionid in characterconditionids)
            {
                sqlQuery = "SElECT name FROM dbo.Conditions WHERE condition_id = @conditionid;";
                command = new SqlCommand(sqlQuery, conn);
                command.Parameters.AddWithValue("@conditionid", conditionid);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string conditionName = reader["name"].ToString();
                    characterconditions.Add(conditionName);
                }
                reader.Close();
            }
        }
        StatusEffectPicker.ItemsSource = statusnames;
        foreach(string selectedEffect in characterconditions)
        {
            int index = statusnames.IndexOf(selectedEffect);
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
        string selectedEffect = StatusEffectPicker.SelectedItem.ToString();
        int index = statusnames.IndexOf(selectedEffect);
        Connection connection = Connection.connectionSingleton;
        int conditionID = -1;
        int characterID = -1;
        if (StatusEffectPicker.SelectedItem == null)
        {
            await DisplayAlert("No Effect Selected", "Please select an effect", "Ok");
            return;
        }
        

        if (existingstatuses.Contains(selectedEffect))
        {
            await DisplayAlert("Effect already present", "Please select a different effect", "Ok");
            return;
        }
        

        try
        {
            using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
            {
                string query = "SELECT condition_id FROM Conditions WHERE name = @SelectedEffect;";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@SelectedEffect", selectedEffect);
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            conditionID = Convert.ToInt32(result);
                        }
                    }
                }
                query = "SELECT CharacterID FROM CharacterSheet WHERE CharacterName = @CharacterName;";
                command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CharacterName", currentcharacterSheet.charactername);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    // No need to create another SqlCommand here
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        characterID = Convert.ToInt32(result);
                    }
                }
                query = "INSERT INTO CharacterConditions (CharacterID, ConditionID) VALUES (@CharacterID, @ConditionID);";
                command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CharacterID", characterID);
                command.Parameters.AddWithValue("@ConditionID", conditionID);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception eSql)
        {
            DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
        }
        DisplayAlert("alert", conditionID.ToString() + " " + characterID.ToString(), "ok");
        

       

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
    /*
    private async void AddEffectButtonClicked(object sender, EventArgs e)
{
    if (StatusEffectPicker.SelectedItem == null)
    {
        await DisplayAlert("No Effect Selected", "Please select an effect", "Ok");
        return;
    }

    string selectedEffect = StatusEffectPicker.SelectedItem.ToString();

    if (existingstatuses.Contains(selectedEffect))
    {
        await DisplayAlert("Effect already present", "Please select a different effect", "Ok");
        return;
    }

    // Add the selected effect to the character
    if (selectedEffect == "Exhaustion")
    {
        // Handle exhaustion differently, as it requires selecting a level
        await DisplayAlert("Select Exhaustion Level", "Please select an exhaustion level", "Ok");
        return;
    }
    else
    {
        // Insert the selected effect into the database
        int characterID = GetCharacterID(); // You need to implement this method to get the current character ID
        int conditionID = GetConditionID(selectedEffect); // You need to implement this method to get the ID of the selected condition

        if (characterID == -1 || conditionID == -1)
        {
            await DisplayAlert("Error", "Failed to add effect", "Ok");
            return;
        }

        // Insert the effect into the CharacterConditions table
        try
        {
            using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
            {
                string insertQuery = "INSERT INTO CharacterConditions (CharacterID, ConditionID) VALUES (@CharacterID, @ConditionID);";
                SqlCommand command = new SqlCommand(insertQuery, conn);
                command.Parameters.AddWithValue("@CharacterID", characterID);
                command.Parameters.AddWithValue("@ConditionID", conditionID);

                conn.Open();
                command.ExecuteNonQuery();
            }

            // Display success message
            await DisplayAlert("Success", "Effect added successfully", "Ok");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            await DisplayAlert("Error", "Failed to add effect", "Ok");
        }
    }
}

// Method to get the character ID (replace with your actual implementation)
private int GetCharacterID()
{
    // Implement logic to get the current character ID
    return 1; // For example, return the ID of the current character
}

// Method to get the condition ID based on the selected effect (replace with your actual implementation)
private int GetConditionID(string selectedEffect)
{
    // Implement logic to get the ID of the selected condition
    // For example, you could query the Conditions table by name
    // and return the corresponding ID
    int conditionID = -1; // Default value if condition is not found

    try
    {
        using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
        {
            string query = "SELECT ConditionID FROM Conditions WHERE ConditionName = @SelectedEffect;";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@SelectedEffect", selectedEffect);

            conn.Open();
            object result = command.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                conditionID = Convert.ToInt32(result);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    return conditionID;
}
*/

    private async void RemoveButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is UserItem userItem)
        {
            int eTypeId = userItem.eTypeId;
            int id = userItem.Id;
            inv.RemoveEquipment(userItem.Id, userItem.eTypeId);
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
    private void LoadCharacterSheetPage(CharacterSheet characterSheet)
    {

        if(currentcharacterSheet.charactername != null)
            User_Disp.Text = "Welcome " + currentcharacterSheet.charactername;
        
        int StrMod = CalcStatMod(currentcharacterSheet.strength);
        int DexMod = CalcStatMod(currentcharacterSheet.dexterity);
        int ConstMod = CalcStatMod(currentcharacterSheet.constitution);
        int IntMod = CalcStatMod(currentcharacterSheet.intelligence);
        int WisMod = CalcStatMod(currentcharacterSheet.wisdom);
        int CharMod = CalcStatMod(currentcharacterSheet.charisma);

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
