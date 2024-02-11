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