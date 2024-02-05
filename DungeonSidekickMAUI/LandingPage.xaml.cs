namespace DungeonSidekickMAUI;

/*
 * 
 *  
 *  Safe keeping in case a header makes more sense in the .xaml of this file
 * */
public partial class LandingPage : ContentPage
{
    CharacterSheet currentcharacterSheet;
    public LandingPage(CharacterSheet passedCharacterSheet)
	{
        currentcharacterSheet = passedCharacterSheet;
		InitializeComponent();

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

        Str_Mod.Text = strmod.ToString();
        Dex_Mod.Text = dexmod.ToString();
        Const_Mod.Text = constmod.ToString();
        Int_Mod.Text = intmod.ToString();
        Wis_Mod.Text = wismod.ToString();
        Char_Mod.Text = charmod.ToString();
    }
}