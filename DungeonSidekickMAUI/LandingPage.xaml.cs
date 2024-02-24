namespace DungeonSidekickMAUI;
public partial class LandingPage : ContentPage
{
    CharacterSheet currentcharacterSheet = CharacterSheet.Instance;
    DiceRoll diceroller;
    public LandingPage()
	{
		InitializeComponent();
        diceroller = new DiceRoll();
        if (currentcharacterSheet != null ) 
        {
            LoadCharacterSheetPage(currentcharacterSheet);
        }
        else
        {
            DisplayAlert("Your character sheet didn't convert correctly", "Let's retry making one", "Ok"); //In case the character sheet breaks
            Navigation.PushAsync(new MainPage()); //at some point during the programming process
        }
	}
    /*
     * Function: CalcStatMod
     * Author: Brendon Williams
     * Purpose: Does the math to calculate a stat's modifier
     * Last Modified: 2/10/2024 12:04pm by Author
     */
    private string CalcStatMod(int stat)
    {
        int mod = stat - 10;
        double truemod = Math.Floor(mod / 2.0); //Double.Floor kind of sucks. Math.Floor is the only way I could get correct negative rounding
        return truemod.ToString();
    }

    /*
     * Function: LoadCharacterSheetPage
     * Author: Brendon Williams
     * Purpose: Helper function that calculates the stat modifiers and then saves them as a global variable for later use
     * Last Modified: 2/24/2024 by Author
     */
    private void LoadCharacterSheetPage(CharacterSheet characterSheet)
    {

        if(currentcharacterSheet.charactername != null)
            User_Disp.Text = "Welcome " + currentcharacterSheet.charactername;

        Preferences.Default.Set("StrMod",CalcStatMod(currentcharacterSheet.strength));
        lblStr_Mod.Text = CalcStatMod(currentcharacterSheet.strength).ToString(); //For each stat, CalcStatMod calculates the modifier based on
        Preferences.Default.Set("DexMod", CalcStatMod(currentcharacterSheet.dexterity));//the stat that gets passed.
        lblDex_Mod.Text = CalcStatMod(currentcharacterSheet.dexterity).ToString();
        Preferences.Default.Set("ConMod", CalcStatMod(currentcharacterSheet.constitution));
        lblConst_Mod.Text = CalcStatMod(currentcharacterSheet.constitution).ToString(); //Using preferences to save the various mods
        Preferences.Default.Set("IntMod", CalcStatMod(currentcharacterSheet.intelligence)); //when they are calculated
        lblInt_Mod.Text = CalcStatMod(currentcharacterSheet.intelligence).ToString();
        Preferences.Default.Set("WisMod", CalcStatMod(currentcharacterSheet.wisdom));
        lblWis_Mod.Text = CalcStatMod(currentcharacterSheet.wisdom).ToString();
        Preferences.Default.Set("ChaMod", CalcStatMod(currentcharacterSheet.charisma));
        lblChar_Mod.Text = CalcStatMod(currentcharacterSheet.charisma).ToString();
    }

    // Anthony
    private void AddItem(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddToInventory());
    }
    /*
     * Function: RollButtonClicked
     * Author: Kenny Rapp
     * Purpose: to call the ParseAndRoll function and set its text to the rolled sum
     * last Modified: 2/4/2024 6:21pm By Kenny Rapp
     */
    //private void RollButtonClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        rollbutton.Text = diceroller.Roll(inputentry.Text).ToString();
    //    }
    //    catch (Exception ex) 
    //    {
    //        DisplayAlert("invalid input(s)", "Please fix your input string", "OK");
    //    }
    //}
}