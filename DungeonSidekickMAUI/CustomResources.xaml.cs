using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


namespace DungeonSidekickMAUI;

public partial class CustomResources : ResourceDictionary
{

    public string PRIMARY;
    public string SECONDARY;
    public string TRINARY;
    public string FC;
    public string ACCENT;
    public string ACCESSORY;



    public CustomResources()
    {
        InitializeComponent();
    }

    private static string fileName = "DesignSettings.txt";
    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);


    /*
    Author: Jonathan Raffaelly
    Date created: 1/19/24
    Function name: SaveColors
    Purpose: Takes passed in strings to store color RGB values in a .txt file. 
                Overwrites text file data if it exists, or creates file if not.
    Modifications:  2/4/24 Adjusted to match new color descriptions.
    */
    public void SaveColors(string PRIMARY, string SECONDARY, string TRINARY, string FC, string ACCENT, string ACCESSORY)
    {
        try
        {
            var newLines = new List<string>();


            newLines.Add(PRIMARY);
            newLines.Add(SECONDARY);
            newLines.Add(TRINARY);
            newLines.Add(FC);
            newLines.Add(ACCENT);
            newLines.Add(ACCESSORY);

            File.WriteAllLines(filePath, newLines);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to save colors. Error: " + ex);
        }
    }

    /*
    Author: Jonathan Raffaelly
    Date created: 1/19/24
    Function name: LoadColors
    Purpose: Grabs RGB values from .txt file, stores them in a usable List, and returns. Helper function for GetColors primarily.
    Modifications:  
    */
    public static List<int> LoadColors()
    {

        var numbersList = new List<int>();
        try
        {
            var lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DesignSettings.txt"));
            foreach (var line in lines)
            {
                string[] numbersStr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (string numStr in numbersStr)
                {
                    if (int.TryParse(numStr, out int num))
                    {
                        numbersList.Add(num);
                    }
                    else
                    {
                        Console.WriteLine("Failed to get color data.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load colors. Error: " + ex);
        }
        return numbersList;
    }

    /*
    Author: Jonathan Raffaelly
    Date created: 1/19/24
    Function name: GetColors
    Purpose: Retrieves RGB color values from .txt file and overwrites current ResourceDictionary 
             values to adjust dynamic colors during runtime.
    Modifications:  1/26/24 - 1/28/24 Major adjustment to code. Now stores the colors in a resource dictionary rather than going to a 
                                      LoadDesign function. This function is called on application startup.
                    2/4/24  Adjusted code to match new color descriptions better. 
                    
    */
    public static void GetColors()
    {
        List<int> list = LoadColors();
        var colors = Application.Current.Resources.MergedDictionaries.ToArray()[2]; // need to find a better why to dynamically recall the dictionary

        // Replaces all colors in dictionary with stored values
        if (!colors.IsNullOrEmpty() && list.Count == 18)
        {
            // need to find a better way to handle the number sequences for this, it looks messy
            colors.Remove("PrimaryColor");
            colors.Add("PrimaryColor", Color.FromRgb(list[0], list[1], list[2]));
            colors.Remove("TrinaryColor");
            colors.Add("TrinaryColor", Color.FromRgb(list[3], list[4], list[5]));
            colors.Remove("SecondaryColor");
            colors.Add("SecondaryColor", Color.FromRgb(list[6], list[7], list[8]));
            colors.Remove("FontC");
            colors.Add("FontC", Color.FromRgb(list[9], list[10], list[11]));
            colors.Remove("AccentColor");
            colors.Add("AccentColor", Color.FromRgb(list[12], list[13], list[14]));
            colors.Remove("AccessoryColor");
            colors.Add("AccessoryColor", Color.FromRgb(list[15], list[16], list[17]));
        }
    }
}


//this is a test for a replacement of our depreciated Character Sheet class
public class ImportedCharacterSheet
{
    // for storing in preferences
    public static void Save(ImportedCharacterSheet character)
    {
        Preferences.Default.Set("UserCharacter", JsonSerializer.Serialize(character));
    }

    // for returing to preferences
    public static ImportedCharacterSheet Load()
    {
        return JsonSerializer.Deserialize<ImportedCharacterSheet>(Preferences.Default.Get("UserCharacter", ""));
    }

    public void Export()
    {
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

        string query = "UPDATE dbo.CharacterSheet" +
            " SET CharacterName = @CharacterName, RaceID = @RaceID, ClassID = @ClassID, Level = @Level, Background = @Background, Alignment = @Alignment, PersonalityTraits = @PersonalityTraits, Ideals = @Ideals, Bonds = @Bonds, Flaws = @Flaws," +
            " FeaturesTraits = @FeaturesTraits, Strength = @Strength, Dexterity = @Dexterity, Constitution = @Constitution, Intelligence = @Intelligence, Wisdom = @Wisdom, Charisma = @Charisma, CurrentHP = @CurrentHP, TempHP = @TempHP, AC = @AC, Initiative = @Initiative," +
            " Speed = @Speed, HitDice = @HitDice, StrSave = @StrSave, DexSave = @DexSave, ConSave = @ConSave, IntSave = @IntSave, WisSave = @WisSave, ChaSave = @ChaSave, Acrobatics = @Acrobatics, AnimalHandling = @AnimalHandling, Arcana = @Arcana, Athletics = @Athletics, Deception = @Deception," +
            " History = @History, Insight = @Insight, Intimidation = @Intimidation, Investigation = @Investigation, Medicine = @Medicine, Nature = @Nature, Perception = @Perception, Performance = @Performance, Persuasion = @Persuasion, Religion = @Religion, Sleight = @Sleight, Stealth = @Stealth," +
            " Survival = @Survival, PassiveWisdom = @PassiveWisdom" +
            " WHERE CharacterID = @CharacterID;";


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
                        cmd.Parameters.Add("@CharacterName", SqlDbType.VarChar).Value = c_Name;
                        cmd.Parameters.Add("@RaceID", SqlDbType.Int).Value = c_Race;
                        cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = c_Class;
                        cmd.Parameters.Add("@Level", SqlDbType.Int).Value = c_Level;

                        cmd.Parameters.Add("@Background", SqlDbType.VarChar).Value = c_Background;
                        cmd.Parameters.Add("@Alignment", SqlDbType.VarChar).Value = c_Alignment;
                        cmd.Parameters.Add("@PersonalityTraits", SqlDbType.VarChar).Value = c_PersonalityTraits;
                        cmd.Parameters.Add("@Ideals", SqlDbType.VarChar).Value = c_Ideals;
                        cmd.Parameters.Add("@Bonds", SqlDbType.VarChar).Value = c_Bonds;
                        cmd.Parameters.Add("@Flaws", SqlDbType.VarChar).Value = c_Flaws;
                        cmd.Parameters.Add("@FeaturesTraits", SqlDbType.VarChar).Value = c_FeaturesTraits;

                        cmd.Parameters.Add("@Strength", SqlDbType.Int).Value = c_Strength;
                        cmd.Parameters.Add("@Dexterity", SqlDbType.Int).Value = c_Dexterity;
                        cmd.Parameters.Add("@Constitution", SqlDbType.Int).Value = c_Constitution;
                        cmd.Parameters.Add("@Intelligence", SqlDbType.Int).Value = c_Intelligence;
                        cmd.Parameters.Add("@Wisdom", SqlDbType.Int).Value = c_Wisdom;
                        cmd.Parameters.Add("@Charisma", SqlDbType.Int).Value = c_Charisma;

                        cmd.Parameters.Add("@CurrentHP", SqlDbType.Int).Value = c_CurrentHealth;
                        cmd.Parameters.Add("@TempHP", SqlDbType.Int).Value = c_TemporaryHealth;
                        cmd.Parameters.Add("@AC", SqlDbType.Int).Value = c_ArmorClass;
                        cmd.Parameters.Add("@Initiative", SqlDbType.Int).Value = c_Initiative;
                        cmd.Parameters.Add("@Speed", SqlDbType.Int).Value = c_Speed;
                        cmd.Parameters.Add("@HitDice", SqlDbType.Int).Value = c_HitDice;

                        cmd.Parameters.Add("@StrSave", SqlDbType.Int).Value = c_StrengthSave;
                        cmd.Parameters.Add("@DexSave", SqlDbType.Int).Value = c_DexteritySave;
                        cmd.Parameters.Add("@ConSave", SqlDbType.Int).Value = c_ConstitutionSave;
                        cmd.Parameters.Add("@IntSave", SqlDbType.Int).Value = c_IntelligenceSave;
                        cmd.Parameters.Add("@WisSave", SqlDbType.Int).Value = c_WisdomSave;
                        cmd.Parameters.Add("@ChaSave", SqlDbType.Int).Value = c_CharismaSave;

                        cmd.Parameters.Add("@Acrobatics", SqlDbType.Int).Value = c_Acrobatics;
                        cmd.Parameters.Add("@AnimalHandling", SqlDbType.Int).Value = c_AnimalHandling;
                        cmd.Parameters.Add("@Arcana", SqlDbType.Int).Value = c_Arcana;
                        cmd.Parameters.Add("@Athletics", SqlDbType.Int).Value = c_Athletics; // for some reason this is giving an error???
                        cmd.Parameters.Add("@Deception", SqlDbType.Int).Value = c_Deception;
                        cmd.Parameters.Add("@History", SqlDbType.Int).Value = c_History;
                        cmd.Parameters.Add("@Insight", SqlDbType.Int).Value = c_Insight;
                        cmd.Parameters.Add("@Intimidation", SqlDbType.Int).Value = c_Intimidation;
                        cmd.Parameters.Add("@Investigation", SqlDbType.Int).Value = c_Investigation;
                        cmd.Parameters.Add("@Medicine", SqlDbType.Int).Value = c_Medicine;
                        cmd.Parameters.Add("@Nature", SqlDbType.Int).Value = c_Nature;
                        cmd.Parameters.Add("@Perception", SqlDbType.Int).Value = c_Perception;
                        cmd.Parameters.Add("@Performance", SqlDbType.Int).Value = c_Performance;
                        cmd.Parameters.Add("@Persuasion", SqlDbType.Int).Value = c_Persuasion;
                        cmd.Parameters.Add("@Religion", SqlDbType.Int).Value = c_Religion;
                        cmd.Parameters.Add("@Sleight", SqlDbType.Int).Value = c_SleightOfHand;
                        cmd.Parameters.Add("@Stealth", SqlDbType.Int).Value = c_Stealth;
                        cmd.Parameters.Add("@Survival", SqlDbType.Int).Value = c_Survival;

                        cmd.Parameters.Add("@PassiveWisdom", SqlDbType.Int).Value = c_PassiveWisdom;

                        cmd.Parameters.Add("@CharacterID", SqlDbType.Int).Value = p_CharacterID;

                        cmd.ExecuteNonQuery();

                        Debug.WriteLine("done");
                    }
                }
            }
        }
        catch (Exception eSql)
        {
            //DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
        }

        Save(this);
    }

    //constructor
    public ImportedCharacterSheet(int p_UID, int p_CharacterID)
    {
        this.p_UID = p_UID;
        this.p_CharacterID = p_CharacterID;
    }

    //public int GetUID()
    //{
    //    return p_UID;
    //}
    //public int GetCharacterID()
    //{
    //    return p_CharacterID;
    //}

    public string? c_Name { get; set; }

    public int c_Class { get; set; } //currently stores an id
    public int c_Race { get; set; } //currently stores an id

    public int c_Level { get; set; }

    public string? c_Background { get; set; } //may have other uses later on
    public string? c_Alignment { get; set; }
    public string? c_PersonalityTraits { get; set; }
    public string? c_Ideals { get; set; }
    public string? c_Bonds { get; set; }
    public string? c_Flaws { get; set; }
    public string? c_FeaturesTraits { get; set; }

    public int c_Equipment { get; set; } //currently stores an id


    //character stats
    public int c_Strength { get; set; }
    public int c_Dexterity { get; set; }
    public int c_Constitution { get; set; }
    public int c_Intelligence { get; set; }
    public int c_Wisdom { get; set; }
    public int c_Charisma { get; set; }

    //character combat stats
    public int c_CurrentHealth { get; set; }
    public int c_TemporaryHealth { get; set; }
    public int c_ArmorClass { get; set; }
    public int c_Initiative { get; set; }
    public int c_Speed { get; set; }
    public int c_HitDice { get; set; }

    //saves
    public int c_StrengthSave { get; set; }
    public int c_DexteritySave { get; set; }
    public int c_ConstitutionSave { get; set; }
    public int c_IntelligenceSave { get; set; }
    public int c_WisdomSave { get; set; }
    public int c_CharismaSave { get; set; }

    //skills
    public int c_Acrobatics { get; set; }
    public int c_AnimalHandling { get; set; }
    public int c_Arcana { get; set; }
    public int c_Athletics { get; set; }
    public int c_Deception { get; set; }
    public int c_History { get; set; }
    public int c_Insight { get; set; }
    public int c_Intimidation { get; set; }
    public int c_Investigation { get; set; }
    public int c_Medicine { get; set; }
    public int c_Nature { get; set; }
    public int c_Perception { get; set; }
    public int c_Performance { get; set; }
    public int c_Persuasion { get; set; }
    public int c_Religion { get; set; }
    public int c_SleightOfHand { get; set; }
    public int c_Stealth { get; set; }
    public int c_Survival { get; set; }

    //storing these in a list is going to be slightly better for visibility, without adding too much more complexity to accessing it.
    //public List<String>? c_Proficiencies;
    //public List<String>? c_Equipment; //this may need heavy changes, possibly set this as a key value pair list so that we could access data as needed, and not just the names

    public int c_PassiveWisdom { get; set; }


    //debateable for keeping
    public int p_UID { get; private set; }
    public int p_CharacterID { get; private set; }


}