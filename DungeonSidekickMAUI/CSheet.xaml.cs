using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;

public partial class CSheet : ContentPage
{
    private ImportedCharacterSheet CharacterSheetcurrent = ImportedCharacterSheet.Instance;
    //public DndClass CharacterClass;
    public int Class;
    public int Race;
    private bool m_NewAcc;


    /* Function Name: CSheet constructor
     * Purpose:
     * Creates the CSheet page
     * Precondition:
     * N/A
     * Returns:
     * nothing
     */
    public CSheet(bool newAcc = false)
    {
        InitializeComponent();
        m_NewAcc = newAcc;
        // nav bar setup
        if (!m_NewAcc)
        {
            //Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
            NavigationCommands cmd = new NavigationCommands();
            NavigationPage.SetHasNavigationBar(this, false);
            var customNavBar = cmd.CreateCustomNavigationBar();
            NavigationBar.Children.Add(customNavBar);
        }


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
        //Allignment.Text = characterSheet.c_Alignment;
        Allignment.SelectedItem = characterSheet.c_Alignment;
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
        //CharacterSheetcurrent.c_Alignment = Allignment.Text;
        CharacterSheetcurrent.c_Alignment = (string)Allignment.SelectedItem;
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
        Navigation.PushAsync(new ClassPickerPage(false,CharacterSheetcurrent, m_NewAcc));
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
        Navigation.PushAsync(new RacePickerPage(false, CharacterSheetcurrent, m_NewAcc));
    }


    /*
     * Function: SubmitStats
     * Author: Brendon Williams
     * Purpose: Adds all the data in this page to the character sheet class, then moves to the next page
     * last Modified : 02/22/2024 8:45pm
     */
    private void SubmitStats(object sender, EventArgs e)
    {
        List<string> list = new List<string>();
        list.Add(CName.Text);
        list.Add(Background.Text);
        //list.Add(Allignment.Text);
        list.Add(PTraits.Text); 
        list.Add(Ideals.Text); 
        list.Add(Bonds.Text); 
        list.Add(Flaws.Text);
        list.Add(Traits.Text);
        if (!GlobalFunctions.entryCheck(list, 0))
            return;
        LoadCharacterSheetClass();
        if (CheckValues())
        {
            Navigation.PushAsync(new CSheet_Stats(m_NewAcc));
        }
    }

    /*
     * Function: CheckValues
     * Author: Anthony Rielly
     * Purpose: Checks to see if the data is ready to be sent to the DB
     * last Modified : 02/22/2024 8:45pm
     */
    private bool CheckValues()
    {
        bool pass = true;
        if (CharacterSheetcurrent.c_Name == null)
        {
            DisplayAlert("Error", "Name cannot be null", "OK");
            pass = false;
        }
        else if(CharacterSheetcurrent.c_Name.Length >= 50)
        {
            DisplayAlert("Error", "Name must be less than 50 characters", "OK");
            pass = false;
        }

        if (CharacterSheetcurrent.c_Race == -1)
        {
            DisplayAlert("Error", "Race cannot be null", "OK");
            pass = false;
        }

        if (CharacterSheetcurrent.c_Class == -1)
        {
            DisplayAlert("Error", "Class cannot be null", "OK");
            pass = false;
        }

        if (CharacterSheetcurrent.c_Background == null)
        {
            DisplayAlert("Error", "Background cannot be null", "OK");
            pass = false;
        }
        else if (CharacterSheetcurrent.c_Background.Length >= 50)
        {
            DisplayAlert("Error", "Background must be less than 50 characters", "OK");
            pass = false;
        }

        if (CharacterSheetcurrent.c_Alignment == null)
        {
            DisplayAlert("Error", "Alignment cannot be null", "OK");
            pass = false;
        }
        //else if (CharacterSheetcurrent.c_Alignment.Length >= 50)
        //{
        //    DisplayAlert("Error", "Alignment must be less than 50 characters", "OK");
        //    pass = false;
        //}

        if (CharacterSheetcurrent.c_PersonalityTraits == null)
        {
            DisplayAlert("Error", "Personality traits cannot be null", "OK");
            pass = false;
        }
        else if (CharacterSheetcurrent.c_PersonalityTraits.Length >= 100)
        {
            DisplayAlert("Error", "Personality traits must be less than 100 characters", "OK");
            pass = false;
        }

        if (CharacterSheetcurrent.c_Ideals == null)
        {
            DisplayAlert("Error", "Ideals cannot be null", "OK");
            pass = false;
        }
        else if (CharacterSheetcurrent.c_Ideals.Length >= 100)
        {
            DisplayAlert("Error", "Name must be less than 100 characters", "OK");
            pass = false;
        }

        if (CharacterSheetcurrent.c_Bonds == null)
        {
            DisplayAlert("Error", "Bonds cannot be null", "OK");
            pass = false;
        }
        else if (CharacterSheetcurrent.c_Bonds.Length >= 100)
        {
            DisplayAlert("Error", "Bonds must be less than 100 characters", "OK");
            pass = false;
        }

        if (CharacterSheetcurrent.c_Flaws == null)
        {
            DisplayAlert("Error", "Flaws cannot be null", "OK");
            pass = false;
        }
        else if (CharacterSheetcurrent.c_Flaws.Length >= 100)
        {
            DisplayAlert("Error", "Flaws must be less than 100 characters", "OK");
            pass = false;
        }

        if (CharacterSheetcurrent.c_FeaturesTraits == null)
        {
            DisplayAlert("Error", "Features and traits cannot be null", "OK");
            pass = false;
        }
        else if (CharacterSheetcurrent.c_FeaturesTraits.Length >= 200)
        {
            DisplayAlert("Error", "Features and traits must be less than 200 characters", "OK");
            pass = false;
        }
        return pass;
    }
}