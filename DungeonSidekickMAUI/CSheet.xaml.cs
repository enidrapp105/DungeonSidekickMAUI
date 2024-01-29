using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;

public partial class CSheet : ContentPage
{
    private CharacterSheet CharacterSheetcurrent;
    private string DexterityRolled;
    private string IntelligenceRolled;
    private string CharismaRolled;
    private string StrengthRolled;
    private string WisdomRolled;
    private string ConstitutionRolled;
    public DndClass CharacterClass;


    public CSheet()
    {
        InitializeComponent();
        CharacterSheetcurrent = new CharacterSheet();
        
    }
    
    public CSheet(CharacterSheet characterSheet)
    {
        InitializeComponent();
        CharacterSheetcurrent = characterSheet;
        LoadCharacterSheetPage(characterSheet);
        if (characterSheet.characterclass != null)
        {
            ClassButton.Text = "Selected Class: " + CharacterClass.ClassName;
        }
    }
    private void LoadCharacterSheetPage(CharacterSheet characterSheet)
    {
        PName.Text = characterSheet.playername;
        CName.Text = characterSheet.charactername ;
        Race.Text = characterSheet.race;
        CharacterClass = characterSheet.characterclass;
        Background.Text = characterSheet.background;
        Allignment.Text = characterSheet.alignment;
        PTraits.Text = characterSheet.personalitytraits;
        Ideals.Text = characterSheet.ideals;
        Bonds.Text = characterSheet.bonds;
        Flaws.Text = characterSheet.flaws;
        Traits.Text = characterSheet.featurestraits;
        Inventory.Text = characterSheet.equipment;
        Attacks.Text = characterSheet.attacks;
        Spells.Text = characterSheet.spells;
    }
    private void LoadCharacterSheetClass()
    {
        CharacterSheetcurrent.playername = PName.Text;
        CharacterSheetcurrent.charactername = CName.Text;
        CharacterSheetcurrent.race = Race.Text;
        CharacterSheetcurrent.characterclass = CharacterClass;
        CharacterSheetcurrent.background = Background.Text;
        CharacterSheetcurrent.alignment = Allignment.Text;
        CharacterSheetcurrent.personalitytraits = PTraits.Text;
        CharacterSheetcurrent.ideals = Ideals.Text;
        CharacterSheetcurrent.bonds = Bonds.Text;
        CharacterSheetcurrent.flaws = Flaws.Text;
        CharacterSheetcurrent.featurestraits = Traits.Text;
        CharacterSheetcurrent.equipment = Inventory.Text;
        CharacterSheetcurrent.attacks = Attacks.Text;
        CharacterSheetcurrent.spells = Spells.Text;
    }
    
    
    /*
     * Function: RollForStats
     * Author: Kenny Rapp
     * Purpose: Navigate to the ClassPicker
     * last Modified : 12/04/2023 3:20pm
     */
    private void ClassPickerPage(object sender, EventArgs e)
    {
        LoadCharacterSheetClass();
        Navigation.PushAsync(new ClassPickerPage(CharacterSheetcurrent));
    }

    /*
     * Function: RacePickerPage
     * Author: Anthony Rielly
     * Purpose: Navigate to the RacePicker
     * last Modified : 01/28/2024 5:00pm
     */
    private void RacePickerPage(object sender, EventArgs e)
    {
        LoadCharacterSheetClass();
        Navigation.PushAsync(new RacePickerPage(CharacterSheetcurrent));
    }

    private void SubmitStats(object sender, EventArgs e)
    {

        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

        string query = "INSERT INTO dbo.CharacterSheet" +
            "(CharacterName,PlayerName,Race,Class,Background,Alignment,PersonalityTraits,Ideals,Bonds,Flaws," +
            "FeaturesTraits,Equipment,Proficiencies,Attacks,Spells,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma) VALUES" +
            "(@CharacterName,@PlayerName,@Race,@Class,@Background,@Alignment,@PersonalityTraits,@Ideals,@Bonds," +
            "@Flaws,@FeaturesTraits,@Equipment,@Proficiencies,@Attacks,@Spells,@Strength,@Dexterity,@Constitution,@Intelligence,@Wisdom,@Charisma);";
        
        // Used for updating existing character sheets
        if (CharacterSheetcurrent.exists == true)
        {
            // This will need changed to a ID of some sort when our DB is finalized
            query = "UPDATE dbo.CharacterSheet " +
            "SET Race = @Race, Class = @Class, Background = @Background, Alignment = @Alignment, PersonalityTraits = @PersonalityTraits, Ideals = @Ideals, " +
            "Bonds = @Bonds, Flaws = @Flaws, FeaturesTraits = @FeaturesTraits, Equipment = @Equipment, Proficiencies = @Proficiencies, Attacks = @Attacks, " +
            "Spells = @Spells, Strength = @Strength, Dexterity = @Dexterity, Constitution = @Constitution, Intelligence = @Intelligence, Wisdom = @Wisdom, Charisma = @Charisma " +
            "WHERE CharacterName = @CharacterName;";
        }
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
                        cmd.Parameters.AddWithValue("@PlayerName", PName.Text);
                        cmd.Parameters.AddWithValue("@CharacterName", CName.Text);
                        cmd.Parameters.AddWithValue("@Race", Race.Text);
                        cmd.Parameters.AddWithValue("@Class", CharacterClass.ClassName);                        
                        cmd.Parameters.AddWithValue("@Background", Background.Text);
                        cmd.Parameters.AddWithValue("@Alignment", Allignment.Text);
                        cmd.Parameters.AddWithValue("@PersonalityTraits", PTraits.Text);
                        cmd.Parameters.AddWithValue("@Ideals", Ideals.Text);
                        cmd.Parameters.AddWithValue("@Bonds", Bonds.Text);
                        cmd.Parameters.AddWithValue("@Flaws", Flaws.Text);
                        cmd.Parameters.AddWithValue("@FeaturesTraits", Traits.Text);
                        cmd.Parameters.AddWithValue("@Equipment", Inventory.Text);
                        cmd.Parameters.AddWithValue("@Attacks", Attacks.Text);
                        cmd.Parameters.AddWithValue("@Spells", Spells.Text);
                    }
                }
            }
            DisplayAlert("Success","Character Sheet Saved!","OK");
        }
        catch (Exception eSql)
        {
            DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
        }
        Navigation.PushAsync(new CSheet_Stats(CharacterSheetcurrent));
    }
}