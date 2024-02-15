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
            Navigation.PushAsync(new MainPage(currentcharacterSheet.playername));
        }
	}

    private void LoadCharacterSheetPage(CharacterSheet characterSheet)
    {
        int strmod = (int.Parse(characterSheet.strength) - 10) / 2; //Doing math externally
        int dexmod = (int.Parse(characterSheet.dexterity) - 10) / 2; //Using int to drop decimal / round down
        int constmod = (int.Parse(characterSheet.constitution) - 10) / 2;
        int intmod = (int.Parse(characterSheet.intelligence) - 10) / 2;
        int wismod = (int.Parse(characterSheet.wisdom) - 10) / 2;
        int charmod = (int.Parse(characterSheet.charisma) - 10) / 2;

        if (characterSheet.charactername != null)
            User_Disp.Text = "Welcome to Your Character Sheet " + characterSheet.charactername;
        Str_Mod.Text = strmod.ToString();
        Dex_Mod.Text = dexmod.ToString();
        Const_Mod.Text = constmod.ToString();
        Int_Mod.Text = intmod.ToString();
        Wis_Mod.Text = wismod.ToString();
        Char_Mod.Text = charmod.ToString();
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