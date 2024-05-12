using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace DungeonSidekickMAUI;

using System.Data;
/*
    Author: Jonathan Raffaelly
    Date created: 11/19/23
    Function name: Character Sheet Import
    Purpose: Used to import saved character sheet data
    Modifications:  11/19/23 - Created sheet and added functionality.
                    11/20/23 - Got import function to work properly.
                    12/02/23 - Changed background to black and text to white
                    01/17/23 - Added edit button
                    01/21/23 - Updated variable names
                    02/18/24 - Use Preference.Default instead of hardcoded values
                    02/29/24 - Fully redesigned to handle new character sheet, added new values and functionality
 */


public partial class CSheet_Import : ContentPage
{
    public CSheet_Import()
    {
        InitializeComponent();

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        //NavigationCommands cmd = new NavigationCommands();
        //NavigationPage.SetHasNavigationBar(this, true);
        //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        //NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());

        ImportSheet();
    }

    private void ImportSheet(/*object sender, EventArgs e*/)
    {
        List<ImportedCharacterSheet> c_List = new List<ImportedCharacterSheet>(); //might be able to remove
        Connection connection = Connection.connectionSingleton;

        string query = "SELECT CharacterID, RaceID, ClassID, CharacterName, Background, Alignment, PersonalityTraits, Ideals, Bonds, Flaws," +
            " FeaturesTraits, Equipment, Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma, CurrentHP, TempHP, AC, Initiative," +
            " Speed, HitDice, StrSave, DexSave, ConSave, IntSave, WisSave, ChaSave, Acrobatics, AnimalHandling, Arcana, Athletics, Deception," +
            " History, Insight, Intimidation, Investigation, Medicine, Nature, Perception, Performance, Persuasion, Religion, Sleight, Stealth," +
            " Survival, PassiveWisdom, UID, Level" +
            " FROM dbo.CharacterSheet" +
            " WHERE UID = @UID;";

        int UserId = Preferences.Default.Get("UserId", -1);

        if (UserId == -1) DisplayAlert("You do not have a valid account", "This should never happen", "Ok");
        try
        {
            using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        //cmd.CommandText = CListQuery;

                        cmd.CommandText = query;
                        cmd.Parameters.Add("@UID", SqlDbType.Int).Value = UserId;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            var hasValue = Application.Current.Resources.TryGetValue("FontC", out object fontColor);
                            var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object secondaryColor);

                            while (reader.Read())
                            {
                                //creating character sheet with all data
                                ImportedCharacterSheet Char = new ImportedCharacterSheet(reader.GetInt32(49), reader.GetInt32(0));
                                Char.c_Race = reader.GetInt32(1);
                                Char.c_Class = reader.GetInt32(2);
                                Char.c_Name = reader.GetString(3);
                                Char.c_Background = reader.GetString(4);
                                Char.c_Alignment = reader.GetString(5);
                                Char.c_PersonalityTraits = reader.GetString(6);
                                Char.c_Ideals = reader.GetString(7);
                                Char.c_Bonds = reader.GetString(8);

                                Char.c_Flaws = reader.GetString(9);
                                Char.c_FeaturesTraits = reader.GetString(10);
                                //Char.c_Equipment = reader.GetInt32(11); // may need adjusted
                                Char.c_Strength = reader.GetInt32(12);
                                Char.c_Dexterity = reader.GetInt32(13);
                                Char.c_Constitution = reader.GetInt32(14);
                                Char.c_Intelligence = reader.GetInt32(15);
                                Char.c_Wisdom = reader.GetInt32(16);
                                Char.c_Charisma = reader.GetInt32(17);

                                try
                                {
                                    for (int i = 18; i < 49; i++)
                                    {
                                        reader.GetInt32(i);
                                    }
                                    Char.c_CurrentHealth = reader.GetInt32(18);
                                    Char.c_TemporaryHealth = reader.GetInt32(19);
                                    Char.c_ArmorClass = reader.GetInt32(20);
                                    Char.c_Initiative = reader.GetInt32(21);
                                    Char.c_Speed = reader.GetInt32(22);
                                    Char.c_HitDice = reader.GetInt32(23);
                                    Char.c_StrengthSave = reader.GetInt32(24);
                                    Char.c_DexteritySave = reader.GetInt32(25);
                                    Char.c_ConstitutionSave = reader.GetInt32(26);
                                    Char.c_IntelligenceSave = reader.GetInt32(27);
                                    Char.c_WisdomSave = reader.GetInt32(28);
                                    Char.c_CharismaSave = reader.GetInt32(29);
                                    Char.c_Acrobatics = reader.GetInt32(30);
                                    Char.c_AnimalHandling = reader.GetInt32(31);
                                    Char.c_Arcana = reader.GetInt32(32);
                                    Char.c_Athletics = reader.GetInt32(33);
                                    Char.c_Deception = reader.GetInt32(34);
                                    Char.c_History = reader.GetInt32(35);
                                    Char.c_Insight = reader.GetInt32(36);
                                    Char.c_Intimidation = reader.GetInt32(37);
                                    Char.c_Investigation = reader.GetInt32(38);
                                    Char.c_Medicine = reader.GetInt32(39);
                                    Char.c_Nature = reader.GetInt32(40);
                                    Char.c_Perception = reader.GetInt32(41);
                                    Char.c_Performance = reader.GetInt32(42);
                                    Char.c_Persuasion = reader.GetInt32(43);
                                    Char.c_Religion = reader.GetInt32(44);
                                    Char.c_SleightOfHand = reader.GetInt32(45);
                                    Char.c_Stealth = reader.GetInt32(46);
                                    Char.c_Survival = reader.GetInt32(47);
                                    Char.c_PassiveWisdom = reader.GetInt32(48);
                                    Char.c_Level = reader.GetInt32(50);
                                }
                                catch
                                {
                                    Char.c_CurrentHealth = 1;
                                    Char.c_TemporaryHealth = 1;
                                    Char.c_ArmorClass = 1;
                                    Char.c_Initiative = 1;
                                    Char.c_Speed = 1;
                                    Char.c_HitDice = 1;
                                    Char.c_StrengthSave = 1;
                                    Char.c_DexteritySave = 1;
                                    Char.c_ConstitutionSave = 1;
                                    Char.c_IntelligenceSave = 1;
                                    Char.c_WisdomSave = 1;
                                    Char.c_CharismaSave = 1;
                                    Char.c_Acrobatics = 1;
                                    Char.c_AnimalHandling = 1;
                                    Char.c_Arcana = 1;
                                    Char.c_Athletics = 1;
                                    Char.c_Deception = 1;
                                    Char.c_History = 1;
                                    Char.c_Insight = 1;
                                    Char.c_Intimidation = 1;
                                    Char.c_Investigation = 1;
                                    Char.c_Medicine = 1;
                                    Char.c_Nature = 1;
                                    Char.c_Perception = 1;
                                    Char.c_Performance = 1;
                                    Char.c_Persuasion = 1;
                                    Char.c_Religion = 1;
                                    Char.c_SleightOfHand = 1;
                                    Char.c_Stealth = 1;
                                    Char.c_Survival = 1;
                                    Char.c_PassiveWisdom = 1;
                                    Char.c_Level = 1;
                                }

                                // *********************************
                                // COMMENTED OUT SINCE A LOT ARE NULL VALUES WHEN PULLED FROM DB
                                //Char.c_Flaws = reader.GetString(9);
                                //Char.c_FeaturesTraits = reader.GetString(10);
                                //Char.c_Equipment = reader.GetInt32(11); // may need adjusted
                                //Char.c_Strength = reader.GetInt32(12);
                                //Char.c_Dexterity = reader.GetInt32(13);
                                //Char.c_Constitution = reader.GetInt32(14);
                                //Char.c_Intelligence = reader.GetInt32(15);
                                //Char.c_Wisdom = reader.GetInt32(16);
                                //Char.c_Charisma = reader.GetInt32(17);
                                //Char.c_CurrentHealth = reader.GetInt32(18);
                                //Char.c_TemporaryHealth = reader.GetInt32(19);
                                //Char.c_ArmorClass = reader.GetInt32(20);
                                //Char.c_Initiative = reader.GetInt32(21);
                                //Char.c_Speed = reader.GetInt32(22);
                                //Char.c_HitDice = reader.GetInt32(23);
                                //Char.c_StrengthSave = reader.GetInt32(24);
                                //Char.c_DexteritySave = reader.GetInt32(25);
                                //Char.c_ConstitutionSave = reader.GetInt32(26);
                                //Char.c_IntelligenceSave = reader.GetInt32(27);
                                //Char.c_WisdomSave = reader.GetInt32(28);
                                //Char.c_CharismaSave = reader.GetInt32(29);
                                //Char.c_Acrobatics = reader.GetInt32(30);
                                //Char.c_AnimalHandling = reader.GetInt32(31);
                                //Char.c_Arcana = reader.GetInt32(32);
                                //Char.c_Athletics = reader.GetInt32(33);
                                //Char.c_Deception = reader.GetInt32(34);
                                //Char.c_History = reader.GetInt32(35);
                                //Char.c_Insight = reader.GetInt32(36);
                                //Char.c_Intimidation = reader.GetInt32(37);
                                //Char.c_Investigation = reader.GetInt32(38);
                                //Char.c_Medicine = reader.GetInt32(39);
                                //Char.c_Nature = reader.GetInt32(40);
                                //Char.c_Perception = reader.GetInt32(41);
                                //Char.c_Performance = reader.GetInt32(42);
                                //Char.c_Persuasion = reader.GetInt32(43);
                                //Char.c_Religion = reader.GetInt32(44);
                                //Char.c_SleightOfHand = reader.GetInt32(45);
                                //Char.c_Stealth = reader.GetInt32(46);
                                //Char.c_Survival = reader.GetInt32(47);
                                //Char.c_PassiveWisdom = reader.GetInt32(48);
                                //Char.c_Level = reader.GetInt32(50);
                                //***********************************

                                //adds new character to character list MIGHT NOT BE NEEDED
                                c_List.Add(Char);

                                //creating visual example of new Character sheet for user interface to choose from
                                StackLayout CharacterStack = new StackLayout();

                                Label CName = new Label();
                                CName.TextColor = (Color)fontColor;
                                CName.Text = "Character Name";
                                Label CNameVal = new Label();
                                CNameVal.TextColor = (Color)fontColor;
                                CNameVal.Text = Char.c_Name;

                                CharacterStack.Children.Add(CName);
                                CharacterStack.Children.Add(CNameVal);

                                Label CRace = new Label();
                                CRace.TextColor = (Color)fontColor;
                                CRace.Text = "Race";
                                Label CRaceVal = new Label();
                                CRaceVal.TextColor = (Color)fontColor;
                                CRaceVal.Text = Char.c_Race.ToString();

                                CharacterStack.Children.Add(CRace);
                                CharacterStack.Children.Add(CRaceVal);

                                Label CClass = new Label();
                                CClass.TextColor = (Color)fontColor;
                                CClass.Text = "Class";
                                Label CClassVal = new Label();
                                CClassVal.TextColor = (Color)fontColor;
                                CClassVal.Text = Char.c_Class.ToString();

                                CharacterStack.Children.Add(CClass);
                                CharacterStack.Children.Add(CClassVal);

                                Label CLevel = new Label();
                                CLevel.TextColor = (Color)fontColor;
                                CLevel.Text = "Level";
                                Label CLevelVal = new Label();
                                CLevelVal.TextColor = (Color)fontColor;
                                CLevelVal.Text = Char.c_Level.ToString();

                                CharacterStack.Children.Add(CLevel);
                                CharacterStack.Children.Add(CLevelVal);

                                Button characterButton = new Button
                                {
                                    TextColor = (Color)fontColor,
                                    Text = "Select Character",
                                    BackgroundColor = (Color)secondaryColor,
                                    Command = new Command
                                    (
                                        execute: async () =>
                                        {
                                            ImportedCharacterSheet.Save(Char);
                                            Navigation.PushAsync(new MainPage());
                                        }
                                    )
                                };

                                CharacterStack.Children.Add(characterButton);

                                BoxView line = new BoxView
                                {
                                    Color = Color.FromRgb(255, 255, 255),
                                    HeightRequest = 1,
                                    HorizontalOptions = LayoutOptions.FillAndExpand
                                };
                                CharacterStack.Children.Add(line);

                                CharacterPanel.Children.Add(CharacterStack);

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
