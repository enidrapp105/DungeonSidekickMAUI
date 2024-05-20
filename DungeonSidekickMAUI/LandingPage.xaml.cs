using System;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
namespace DungeonSidekickMAUI;
public partial class LandingPage : ContentPage
{
    ImportedCharacterSheet currentcharacterSheet = ImportedCharacterSheet.Instance;
    ImportedCharacterSheet currentcharacterSheet2 = ImportedCharacterSheet.Load();
    DiceRoll diceroller;
    
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

        

        AC.Text = "AC: " + currentcharacterSheet.c_ACBoost.ToString();

        Connection connection = Connection.connectionSingleton;
        statusnames = new List<string>();
        statusdescriptions = new List<string>();
        existingstatuses = new HashSet<string>();
        exhaustiondescriptions = new List<string>();
        List<int> characterconditionids = new List<int>();
        List<string> characterconditions = new List<string>();


        using (SqlConnection conn = new SqlConnection(connection.connectionString))
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
            command.Parameters.AddWithValue("@CharacterName", currentcharacterSheet2.c_Name);
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
        using (SqlConnection conn = new SqlConnection(connection.connectionString))
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
            int selectedeffectid = -1;
            using (SqlConnection conn = new SqlConnection(connection.connectionString))
            {
                string sqlQuery = "SELECT condition_id FROM dbo.Conditions WHERE name = @name;";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                command.Parameters.AddWithValue("@name", selectedEffect); // Set parameter value
                conn.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Check if there are rows
                    {
                        // Read the value from the reader
                        selectedeffectid = (int)reader["condition_id"];
                    }
                }
            }
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
                removeButton.Clicked += async (s, args) =>
                {
                    statusstack.Children.Remove(exhaustionView);
                    statusstack.Children.Remove(removeButton);
                    existingstatuses.Remove(selectedEffect);
                    // Capture the IDs before removing
                    int charID = characterid;
                    int condID = 15;

                    // Remove from database
                    await RemoveStatusFromDatabase(charID, condID);
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
                    removeButton.Clicked += async (s, args) =>
                    {
                        statusstack.Children.Remove(effectLabel);
                        statusstack.Children.Remove(removeButton);
                        existingstatuses.Remove(selectedEffect);
                        // Capture the IDs before removing
                        int charID = characterid;
                        int condID = selectedeffectid;

                        // Remove from database
                        await RemoveStatusFromDatabase(charID, condID);
                    };

                    statusstack.Children.Add(effectLabel);
                    statusstack.Children.Add(removeButton);
                    existingstatuses.Add(selectedEffect);
                }
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
            using (SqlConnection conn = new SqlConnection(connection.connectionString))
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
                command.Parameters.AddWithValue("@CharacterName", currentcharacterSheet2.c_Name);
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
            removeButton.Clicked += async (s, args) =>
            {
                statusstack.Children.Remove(exhaustionView);
                statusstack.Children.Remove(removeButton);
                existingstatuses.Remove(selectedEffect);
                // Capture the IDs before removing
                int charID = characterID;
                int condID = conditionID;

                // Remove from database
                await RemoveStatusFromDatabase(charID, condID);
                
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
                removeButton.Clicked += async (s, args) =>
                {
                    statusstack.Children.Remove(effectLabel);
                    statusstack.Children.Remove(removeButton);
                    existingstatuses.Remove(selectedEffect);
                    // Capture the IDs before removing
                    int charID = characterID;
                    int condID = conditionID;

                    // Remove from database
                    await RemoveStatusFromDatabase(charID, condID);
                };

                statusstack.Children.Add(effectLabel);
                statusstack.Children.Add(removeButton);
                existingstatuses.Add(selectedEffect);
            }
        }
        
    }
    async Task RemoveStatusFromDatabase(int charID, int condID)
    {
        Connection connection = Connection.connectionSingleton;
        try
        {
            using (SqlConnection conn = new SqlConnection(connection.connectionString))
            {
                string query = "DELETE FROM dbo.CharacterConditions WHERE CharacterID = @CharacterID AND ConditionID = @ConditionID";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CharacterID", charID);
                command.Parameters.AddWithValue("@ConditionID", condID);
                await conn.OpenAsync(); // Open asynchronously
                await command.ExecuteNonQueryAsync(); // Execute asynchronously
            }
        }
        catch (Exception eSql)
        {
            await DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
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
