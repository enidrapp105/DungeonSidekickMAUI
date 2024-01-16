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
    public CSheet(string DEX, string INT, string CHA, string STR, string WIS, string CON, DndClass dndclass)
    {
        DexterityRolled = DEX;
        IntelligenceRolled = INT;
        CharismaRolled = CHA;
        StrengthRolled = STR;
        WisdomRolled = WIS;
        ConstitutionRolled = CON;
        CharacterClass = dndclass;
        InitializeComponent();
        Dexterity.Text = DEX;
        Intelligence.Text = INT;
        Charisma.Text = CHA;
        Strength.Text = STR;
        Wisdom.Text = WIS;
        Constitution.Text = CON;
        ClassButton.Text = "Selected Class: " + CharacterClass.ClassName;
    }
    public CSheet(CharacterSheet characterSheet)
    {
        InitializeComponent();
        LoadCharacterSheetPage(characterSheet);
        ClassButton.Text = "Selected Class: " + CharacterClass.ClassName;
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
        Proficiencies.Text = characterSheet.proficiencies;
        Attacks.Text = characterSheet.attacks;
        Spells.Text = characterSheet.spells;
        Strength.Text = characterSheet.strength;
        Dexterity.Text = characterSheet.dexterity;
        Constitution.Text = characterSheet.constitution;
        Intelligence.Text = characterSheet.intelligence;
        Wisdom.Text = characterSheet.wisdom;
        Constitution.Text = characterSheet.constitution;
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
        CharacterSheetcurrent.proficiencies = Proficiencies.Text;
        CharacterSheetcurrent.attacks = Attacks.Text;
        CharacterSheetcurrent.spells = Spells.Text;
        CharacterSheetcurrent.strength = Strength.Text;
        CharacterSheetcurrent.dexterity = Dexterity.Text;
        CharacterSheetcurrent.constitution = Constitution.Text;
        CharacterSheetcurrent.intelligence = Intelligence.Text;
        CharacterSheetcurrent.wisdom = Wisdom.Text;
        CharacterSheetcurrent.constitution = Constitution.Text;

    }
    /*
     * Function: RollForStats
     * Author: Kenny Rapp
     * Purpose: Navigate to the RollForStats
     * last Modified : 11/19/2023 3:25pm
     */
    private void RollForStats(object sender, EventArgs e)
    {
        if (ClassButton.Text == "Pick your Class")
            DisplayAlert("Error", "Please select yor Class", "Ok");
        else
            Navigation.PushAsync(new RollForStatsPage(CharacterClass));
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


    private void SubmitStats(object sender, EventArgs e)
    {

        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6";

        string query = "INSERT INTO dbo.CharacterSheet" +
            "(CharacterName,PlayerName,Race,Class,Background,Alignment,PersonalityTraits,Ideals,Bonds,Flaws," +
            "FeaturesTraits,Equipment,Proficiencies,Attacks,Spells,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma) VALUES" +
            "(@CharacterName,@PlayerName,@Race,@Class,@Background,@Alignment,@PersonalityTraits,@Ideals,@Bonds," +
            "@Flaws,@FeaturesTraits,@Equipment,@Proficiencies,@Attacks,@Spells,@Strength,@Dexterity,@Constitution,@Intelligence,@Wisdom,@Charisma);";

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
                        cmd.Parameters.AddWithValue("@Proficiencies", Proficiencies.Text);
                        cmd.Parameters.AddWithValue("@Attacks", Attacks.Text);
                        cmd.Parameters.AddWithValue("@Spells", Spells.Text);
                        int flag = 0;

                        if (int.Parse(Strength.Text) >= 0 && int.Parse(Strength.Text) <= 18)
                            cmd.Parameters.AddWithValue("@Strength", int.Parse(Strength.Text));
                        else
                            flag = 1;

                        if (int.Parse(Dexterity.Text) >= 0 && int.Parse(Dexterity.Text) <= 18)
                            cmd.Parameters.AddWithValue("@Dexterity", int.Parse(Dexterity.Text));
                        else
                            flag = 1;

                        if (int.Parse(Constitution.Text) >= 0 && int.Parse(Constitution.Text) <= 18)
                            cmd.Parameters.AddWithValue("@Constitution", int.Parse(Constitution.Text));
                        else
                            flag = 1;

                        if (int.Parse(Intelligence.Text) >= 0 && int.Parse(Intelligence.Text) <= 18)
                            cmd.Parameters.AddWithValue("@Intelligence", int.Parse(Intelligence.Text));
                        else
                            flag = 1;

                        if (int.Parse(Wisdom.Text) >= 0 && int.Parse(Wisdom.Text) <= 18)
                            cmd.Parameters.AddWithValue("@Wisdom", int.Parse(Wisdom.Text));
                        else
                            flag = 1;

                        if (int.Parse(Charisma.Text) >= 0 && int.Parse(Charisma.Text) <= 18)
                            cmd.Parameters.AddWithValue("@Charisma", int.Parse(Charisma.Text));
                        else
                            flag = 1;

                        if (flag == 0)
                            cmd.ExecuteNonQuery();
                        else
                            Console.WriteLine("One of your stats is either below 0 or above 20, please move it to between this range.");
                    }
                }
            }
        }
        catch (Exception eSql)
        {
            Debug.WriteLine("Exception: " + eSql.Message);
        }
    }
}