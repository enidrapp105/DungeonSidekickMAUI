using Microsoft.IdentityModel.Tokens;


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


//this is a possibly temporary test for a character sheet that might be used re-access a character between pages without having to connect to the database every time
public class CharacterSheet
{

    private
    int p_UID; //debateable for keeping

    //character flavor text, most likely will have no impact on usability other than for a visual representation.    
    string c_Name = "";
    string c_Background = ""; //may have other uses in the future
    string c_Alignment = "";
    string c_PersonalityTraits = "";
    string c_Ideals = "";
    string c_Bonds = "";
    string c_Flaws = "";
    string c_FeaturesTraits = "";

    //these are the IDs. may also want to swap to string with actual names instead. this depends on how we want to work with the data. 
    int c_Class; 
    int c_Race;

    //character stats
    int c_Str;
    int c_Dex;
    int c_Const;
    int c_Int;
    int c_Wis;
    int c_Charisma;

    //character combat stats
    int c_CurrHP;
    int c_TempHP;
    int c_AC;
    int c_Initiative;
    int c_Speed;
    int c_HitDice;

    //saves
    int c_StrSave;
    int c_DexSave;
    int c_ConstSave;
    int c_IntSave;
    int c_WisSave;
    int c_CharismaSave;

    //skills
    int c_Acrobatics;
    int c_AnimalHandling;
    int c_Arcana;
    int c_Atheletics;
    int c_Deception;
    int c_History;
    int c_Insight;
    int c_Intimidation;
    int c_Investigation;
    int c_Medicine;
    int c_Nature;
    int c_Perception;
    int c_Performance;
    int c_Persuasion;
    int c_Religion;
    int c_SleightOfHand;
    int c_Stealth;
    int c_Survival;
}