using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;

public partial class CSheet : ContentPage
{
    private ImportedCharacterSheet CharacterSheetcurrent = ImportedCharacterSheet.Instance;
    //public DndClass CharacterClass;
    public int Class;
    public int Race;


    /* Function Name: CSheet constructor
     * Purpose:
     * Creates the CSheet page
     * Precondition:
     * N/A
     * Returns:
     * nothing
     */
    public CSheet()
    {
        InitializeComponent();

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, true);
        ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());

        LoadCharacterSheetPage(CharacterSheetcurrent);
        if (CharacterSheetcurrent.c_ClassName != null)
        {
            ClassButton.Text = "Selected Class: " + CharacterSheetcurrent.c_ClassName;
        }
        if (CharacterSheetcurrent.c_RaceName != null)
        {
            RaceButton.Text = "Selected Race: " + CharacterSheetcurrent.c_RaceName;
        }
    }
    /* Function Name: LoadCharacterSheetPage
     * Purpose:
     * to assign all of the values that are in the text fields from the passed in
     * character sheet class to the csheet page
     * Precondition:
     * pass in a charactersheet object
     * Returns:
     * nothing
     */
    private void LoadCharacterSheetPage(ImportedCharacterSheet characterSheet)
    {
        CName.Text = characterSheet.c_Name;
        Race = characterSheet.c_Race;
        Class = characterSheet.c_Class;
        Background.Text = characterSheet.c_Background;
        Allignment.Text = characterSheet.c_Alignment;
        PTraits.Text = characterSheet.c_PersonalityTraits;
        Ideals.Text = characterSheet.c_Ideals;
        Bonds.Text = characterSheet.c_Bonds;
        Flaws.Text = characterSheet.c_Flaws;
        Traits.Text = characterSheet.c_FeaturesTraits;
    }
    /* Function Name: LoadCharacterSheetClass
     * Purpose:
     * to assign all of the values that are in the text fields to the character sheet object
     * Precondition:
     * nothing
     * Returns:
     * nothing
     */
    private void LoadCharacterSheetClass()
    {
        CharacterSheetcurrent.c_Name = CName.Text;
        CharacterSheetcurrent.c_Race = Race;
        CharacterSheetcurrent.c_Class = Class;
        CharacterSheetcurrent.c_Background = Background.Text;
        CharacterSheetcurrent.c_Alignment = Allignment.Text;
        CharacterSheetcurrent.c_PersonalityTraits = PTraits.Text;
        CharacterSheetcurrent.c_Ideals = Ideals.Text;
        CharacterSheetcurrent.c_Bonds = Bonds.Text;
        CharacterSheetcurrent.c_Flaws = Flaws.Text;
        CharacterSheetcurrent.c_FeaturesTraits = Traits.Text;
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


    /*
     * Function: SubmitStats
     * Author: Brendon Williams
     * Purpose: Adds all the data in this page to the character sheet class, then moves to the next page
     * last Modified : 02/22/2024 8:45pm
     */
    private void SubmitStats(object sender, EventArgs e)
    {
        LoadCharacterSheetClass();
        Navigation.PushAsync(new CSheet_Stats());
    }
}