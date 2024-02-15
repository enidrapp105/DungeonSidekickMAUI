namespace DungeonSidekickMAUI;
public partial class LandingPage : ContentPage
{
    CharacterSheet currentcharacterSheet;
    DiceRoll diceroller;
    public LandingPage(CharacterSheet passedCharacterSheet)
	{
        currentcharacterSheet = passedCharacterSheet;
		InitializeComponent();
        diceroller = new DiceRoll();
        if (currentcharacterSheet != null ) 
        {
            LoadCharacterSheetPage(currentcharacterSheet);
        }
        else
        {
            DisplayAlert("Your character sheet didn't convert correctly", "Let's retry making one", "Ok");
            Navigation.PushAsync(new MainPage());
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
     * Purpose: Helper Function that fills in the various text boxes on the landingpage.xaml
     * Last Modified: 2/10/2024 10:35pm by Author
     */
    private void LoadCharacterSheetPage(CharacterSheet characterSheet)
    {

        if(currentcharacterSheet.charactername != null)
            User_Disp.Text = "Welcome " + currentcharacterSheet.charactername;

        lblStr_Mod.Text = CalcStatMod(int.Parse(currentcharacterSheet.strength)); //For each stat, CalcStatMod calculates the modifier based on
        lblDex_Mod.Text = CalcStatMod(int.Parse(currentcharacterSheet.dexterity)); //the stat that gets passed.
        lblConst_Mod.Text = CalcStatMod(int.Parse(currentcharacterSheet.constitution));
        lblInt_Mod.Text = CalcStatMod(int.Parse(currentcharacterSheet.intelligence));
        lblWis_Mod.Text = CalcStatMod(int.Parse(currentcharacterSheet.wisdom));
        lblChar_Mod.Text = CalcStatMod(int.Parse(currentcharacterSheet.charisma));
    }

    /*
     * Function: RollButtonClicked
     * Author: Kenny Rapp
     * Purpose: to call the ParseAndRoll function and set its text to the rolled sum
     * last Modified: 2/4/2024 6:21pm By Kenny Rapp
     */
    private void RollButtonClicked(object sender, EventArgs e)
    {
        try
        {
            rollbutton.Text = diceroller.Roll(inputentry.Text).ToString();
        }
        catch (Exception ex) 
        {
            DisplayAlert("invalid input(s)", "Please fix your input string", "OK");
        }
    }
}