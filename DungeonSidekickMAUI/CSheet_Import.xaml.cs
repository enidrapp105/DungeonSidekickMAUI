using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Collections.Generic;

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
 */

using System.Diagnostics;
using Microsoft.Data.SqlClient;
public partial class CSheet_Import : ContentPage
{
    public CSheet_Import()
    {
        InitializeComponent();

    }
    private void oldImportSheet(object sender, EventArgs e)
    {
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

        string CListQuery = "SELECT CharacterID FROM dbo.CharacterList WHERE UID = @UserId;"; //This is to check the lookup for correct tables

        string query = "SELECT CharacterName,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma FROM dbo.CharacterSheet" +
            " WHERE PlayerName = @PlayerName;";

        int UserId = Preferences.Default.Get("UserId", -1);
        if (UserId == -1) DisplayAlert("You do not have a valid account", "This should never happen", "Ok");
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = CListQuery;
                        cmd.Parameters.AddWithValue("@UserId", Preferences.Default.Get("UserId", -1));
                        LinkedList<int> CharacterIDs = new LinkedList<int>();
                        using(SqlDataReader CListreader = cmd.ExecuteReader())
                        {
                            while(CListreader.Read()) 
                            {
                                CharacterIDs.AddFirst(CListreader.GetInt32(0));
                            }
                        }
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@PlayerName", PName.Text);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            var hasValue = Application.Current.Resources.TryGetValue("FontC", out object fontColor);

                            var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object frameColor);

                            while (reader.Read())
                            {
                                StackLayout CharacterStack = new StackLayout();

                                Label CName = new Label();
                                CName.TextColor = (Color)fontColor;
                                CName.Text = "Character Name";
                                Label CNameVal = new Label();
                                CNameVal.TextColor = (Color)fontColor;
                                CNameVal.Text = reader.GetString(0);

                                CharacterStack.Children.Add(CName);
                                CharacterStack.Children.Add(CNameVal);

                                Label Strength = new Label();
                                Strength.TextColor = (Color)fontColor;
                                Strength.Text = "Strength";
                                Label StrengthVal = new Label();
                                StrengthVal.TextColor = (Color)fontColor;
                                StrengthVal.Text = reader.GetInt32(1).ToString();
                                CharacterStack.Children.Add(Strength);
                                CharacterStack.Children.Add(StrengthVal);

                                Label Dexterity = new Label();
                                Dexterity.TextColor = (Color)fontColor;
                                Dexterity.Text = "Dexterity";
                                Label DexterityVal = new Label();
                                DexterityVal.TextColor = (Color)fontColor;
                                DexterityVal.Text = reader.GetInt32(2).ToString();
                                CharacterStack.Children.Add(Dexterity);
                                CharacterStack.Children.Add(DexterityVal);

                                Label Constitution = new Label();
                                Constitution.TextColor = (Color)fontColor   ;
                                Constitution.Text = "Constitution";
                                Label ConstitutionVal = new Label();
                                ConstitutionVal.TextColor = (Color)fontColor;
                                ConstitutionVal.Text = reader.GetInt32(3).ToString();

                                Label Intelligence = new Label();
                                Intelligence.TextColor = (Color)fontColor;
                                Intelligence.Text = "Intelligence";
                                Label IntelligenceVal = new Label();
                                IntelligenceVal.TextColor = (Color)fontColor;
                                IntelligenceVal.Text = reader.GetInt32(4).ToString();
                                CharacterStack.Children.Add(Constitution);
                                CharacterStack.Children.Add(ConstitutionVal);

                                Label Wisdom = new Label();
                                Wisdom.TextColor = (Color)fontColor;
                                Wisdom.Text = "Wisdom";
                                Label WisdomVal = new Label();
                                WisdomVal.TextColor = (Color)fontColor;
                                WisdomVal.Text = reader.GetInt32(5).ToString();
                                CharacterStack.Children.Add(Wisdom);
                                CharacterStack.Children.Add(WisdomVal);

                                Label Charisma = new Label();
                                Charisma.TextColor = (Color)fontColor;
                                Charisma.Text = "Charisma";
                                Label CharismaVal = new Label();
                                CharismaVal.TextColor = (Color)fontColor;
                                CharismaVal.Text = reader.GetInt32(6).ToString();
                                CharacterStack.Children.Add(Charisma);
                                CharacterStack.Children.Add(CharismaVal);

                                Button Edit = new Button {
                                    TextColor = (Color)fontColor,
                                    Text = "Edit",
                                    BackgroundColor = (Color)frameColor,
                                    Command = new Command
                                    (
                                        execute: async() =>
                                        {
                                            CharacterSheet sheet = new CharacterSheet();
                                            sheet.playername = PName.Text;
                                            sheet.charactername = CNameVal.Text;
                                            sheet.strength = StrengthVal.Text;
                                            sheet.dexterity = DexterityVal.Text;
                                            sheet.constitution = ConstitutionVal.Text;
                                            sheet.intelligence = IntelligenceVal.Text;
                                            sheet.wisdom = WisdomVal.Text;
                                            sheet.charisma = CharismaVal.Text;
                                            sheet.exists = true;
                                            Navigation.PushAsync(new CSheet(sheet));
                                        }
                                    )
                                };
                                CharacterStack.Children.Add(Edit);
                                BoxView line = new BoxView
                                {
                                    Color = Color.FromRgb(255,255,255),
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
    private void ImportSheet(object sender, EventArgs e)
    {
        List<ImportedCharacterSheet> c_List = new List<ImportedCharacterSheet>(); //might be able to remove
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                                Char.c_Equipment = reader.GetInt32(11); // may need adjusted
                                Char.c_Str = reader.GetInt32(12);
                                Char.c_Dex = reader.GetInt32(13);
                                Char.c_Const = reader.GetInt32(14);
                                Char.c_Int = reader.GetInt32(15);
                                Char.c_Wis = reader.GetInt32(16);
                                Char.c_Charisma = reader.GetInt32(17);
                                Char.c_CurrHP = reader.GetInt32(18);
                                Char.c_TempHP = reader.GetInt32(19);
                                Char.c_AC = reader.GetInt32(20);
                                Char.c_Initiative = reader.GetInt32(21);
                                Char.c_Speed = reader.GetInt32(22);
                                Char.c_HitDice = reader.GetInt32(23);
                                Char.c_StrSave = reader.GetInt32(24);
                                Char.c_DexSave = reader.GetInt32(25);
                                Char.c_ConstSave = reader.GetInt32(26);
                                Char.c_IntSave = reader.GetInt32(27);
                                Char.c_WisSave = reader.GetInt32(28);
                                Char.c_CharismaSave = reader.GetInt32(29);
                                Char.c_Acrobatics = reader.GetInt32(30);
                                Char.c_AnimalHandling = reader.GetInt32(31);
                                Char.c_Arcana = reader.GetInt32(32);
                                Char.c_Atheletics = reader.GetInt32(33);
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
                                Char.c_PasWis = reader.GetInt32(48);
                                Char.c_Level = reader.GetInt32(50);

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
                                            //Navigate to next page
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
