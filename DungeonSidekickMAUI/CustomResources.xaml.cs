using Microsoft.IdentityModel.Tokens;
using System.Text.Json;


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

    //constructor
    public ImportedCharacterSheet(int p_UID, int p_CharacterID)
    {
        this.p_UID = p_UID;
        this.p_CharacterID = p_CharacterID;
    }

    public int GetUID()
    {
        return p_UID;
    }
    public int GetCharacterID()
    {
        return p_CharacterID;
    }

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
    public int c_Atheletics { get; set; }
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