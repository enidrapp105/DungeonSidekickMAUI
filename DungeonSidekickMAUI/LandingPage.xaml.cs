namespace DungeonSidekickMAUI;


public partial class LandingPage : ContentPage
{
    CharacterSheet currentcharacterSheet;
    static Random random = new Random();
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
            Navigation.PushAsync(new MainPage(" "));
        }
	}

    private void LoadCharacterSheetPage(CharacterSheet characterSheet)
    {
        int strmod = (int.Parse(characterSheet.strength) - 10) / 2; //Doing math externally
        int dexmod = (int.Parse(characterSheet.dexterity) - 10) / 2; //Using int to drop decimal
        int constmod = (int.Parse(characterSheet.constitution) - 10) / 2;
        int intmod = (int.Parse(characterSheet.intelligence) - 10) / 2;
        int wismod = (int.Parse(characterSheet.wisdom) - 10) / 2;
        int charmod = (int.Parse(characterSheet.charisma) - 10) / 2;

        if(currentcharacterSheet.charactername != null)
            User_Disp.Text = "Welcome " + currentcharacterSheet.charactername;

        Str_Mod.Text = strmod.ToString();
        Dex_Mod.Text = dexmod.ToString();
        Const_Mod.Text = constmod.ToString();
        Int_Mod.Text = intmod.ToString();
        Wis_Mod.Text = wismod.ToString();
        Char_Mod.Text = charmod.ToString();
    }
    /*
     * Function: RollDice
     * Author: Kenny Rapp
     * Purpose: to roll dice and return the rolled sum
     * last Modified: 2/4/2024 6:21pm By Kenny Rapp
     */
    public int RollDice(int numberOfDice, int numberOfFaces)
    {
        int totalSum = 0;

        for (int i = 0; i < numberOfDice; i++)
        {
            int rollResult = random.Next(1, numberOfFaces + 1);
            totalSum += rollResult;
        }

        return totalSum;
    }
    /*
     * Function: ParseAndRoll
     * Author: Kenny Rapp
     * Purpose: to parse the input string and return the rolled sum
     * last Modified: 2/4/2024 6:21pm By Kenny Rapp
     */
    public int ParseAndRoll(string input)
    {
        try
        {
            string[] parts = input.Split(new char[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries);

            int totalResult = 0;

            foreach (var part in parts)
            {
                if (part.Contains("d"))
                {
                    string[] dicePart = part.Split('d');
                    int numberOfDice = int.Parse(dicePart[0]);
                    int numberOfFaces = int.Parse(dicePart[1]);

                    totalResult += RollDice(numberOfDice, numberOfFaces);
                }
                else
                {
                    int modifier = int.Parse(part);
                    totalResult += modifier;
                }
            }

            return totalResult;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Invalid input: {ex.Message}", "OK");
            return 0; // or any default value
        }
    }
    /*
     * Function: RollButtonClicked
     * Author: Kenny Rapp
     * Purpose: to call the ParseAndRoll function and set its text to the rolled sum
     * last Modified: 2/4/2024 6:21pm By Kenny Rapp
     */
    private void RollButtonClicked(object sender, EventArgs e) 
    {
        //rollbutton.Text = ParseAndRoll(inputentry.Text).ToString();
    }
}