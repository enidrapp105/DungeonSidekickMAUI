using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;

public partial class CSheet : ContentPage
{
    private CharacterSheet CharacterSheetcurrent = CharacterSheet.Instance;
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
        if (CharacterSheetcurrent.className != null)
        {
            ClassButton.Text = "Selected Class: " + CharacterSheetcurrent.className;
        }
        if (CharacterSheetcurrent.raceName != null)
        {
            RaceButton.Text = "Selected Race: " + CharacterSheetcurrent.raceName;
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
    private void LoadCharacterSheetPage(CharacterSheet characterSheet)
    {
        CName.Text = characterSheet.charactername ;
        Race = characterSheet.race;
        Class = characterSheet.characterclass;
        Background.Text = characterSheet.background;
        Allignment.Text = characterSheet.alignment;
        PTraits.Text = characterSheet.personalitytraits;
        Ideals.Text = characterSheet.ideals;
        Bonds.Text = characterSheet.bonds;
        Flaws.Text = characterSheet.flaws;
        Traits.Text = characterSheet.featurestraits;
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
        CharacterSheetcurrent.charactername = CName.Text;
        CharacterSheetcurrent.race = Race;
        CharacterSheetcurrent.characterclass = Class;
        CharacterSheetcurrent.background = Background.Text;
        CharacterSheetcurrent.alignment = Allignment.Text;
        CharacterSheetcurrent.personalitytraits = PTraits.Text;
        CharacterSheetcurrent.ideals = Ideals.Text;
        CharacterSheetcurrent.bonds = Bonds.Text;
        CharacterSheetcurrent.flaws = Flaws.Text;
        CharacterSheetcurrent.featurestraits = Traits.Text;
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