using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;
public partial class LandingPage : ContentPage
{
    CharacterSheet currentcharacterSheet = CharacterSheet.Instance;
    DiceRoll diceroller;
    Inventory inv;
    public LandingPage()
	{
        // Allows us to use dynamic colors when creating labels, buttons, etc in this function
        Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
        Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
        Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];

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

        inv = new Inventory(1); // TEMP PLACEHOLDER 1
        inv.PullItems();
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        foreach (var weapon in inv.Weapons)
        {
            string query = "SELECT name FROM dbo.Weapon" +
            " WHERE WeaponID = @Id;";
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

                            // grabs ID from weapon list
                            cmd.Parameters.AddWithValue("@Id", weapon[0]);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    HorizontalStackLayout layout = new HorizontalStackLayout();
                                    Label weaponLabel = new Label();
                                    weaponLabel.TextColor = (Color)fontColor;

                                    // grabs the name from the DB and quantity from weapon list
                                    string name = reader.GetString(0);
                                    weaponLabel.Text = name + " x" + weapon[1];
                                    layout.Add(weaponLabel);

                                    // Button that removes the item from the DB
                                    Button delete = new Button
                                    {
                                        TextColor = fontColor,
                                        Text = "Remove",
                                        BackgroundColor = TrinaryColor,
                                        Command = new Command
                                   (
                                       execute: async () =>
                                       {
                                           
                                       }
                                   )
                                    };
                                    layout.Add(delete);
                                    InvStack.Add(layout);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                DisplayAlert("Error!", eSql.Message, "OK");
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }

        foreach (var armor in inv.Equipment)
        {
            string query = "SELECT name FROM dbo.Armor" +
            " WHERE ArmorID = @Id;";
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

                            // grabs ID from equipment list
                            cmd.Parameters.AddWithValue("@Id", armor[0]);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    HorizontalStackLayout layout = new HorizontalStackLayout();
                                    Label armorLabel = new Label();
                                    armorLabel.TextColor = (Color)fontColor;

                                    // grabs the name from the DB and quantity from equipment list
                                    armorLabel.Text = reader.GetString(0) + " x" + armor[1];
                                    layout.Add(armorLabel);

                                    // Button that removes the item from the DB
                                    Button delete = new Button
                                    {
                                        TextColor = fontColor,
                                        Text = "Remove",
                                        BackgroundColor = TrinaryColor,
                                        Command = new Command
                                   (
                                       execute: async () =>
                                       {

                                       }
                                   )
                                    };
                                    layout.Add(delete);
                                    InvStack.Add(layout);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                DisplayAlert("Error!", eSql.Message, "OK");
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }

        foreach (var gear in inv.Items)
        {
            string query = "SELECT name FROM dbo.Gear" +
            " WHERE GearId = @Id;";
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

                            // grabs ID from items list
                            cmd.Parameters.AddWithValue("@Id", gear[0]);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    HorizontalStackLayout layout = new HorizontalStackLayout();
                                    Label gearLabel = new Label();
                                    gearLabel.TextColor = (Color)fontColor;

                                    // grabs the name from the DB and quantity from items list
                                    gearLabel.Text = reader.GetString(0) + " x" + gear[1];
                                    layout.Add(gearLabel);

                                    // Button that removes the item from the DB
                                    Button delete = new Button
                                    {
                                        TextColor = fontColor,
                                        Text = "Remove",
                                        BackgroundColor = TrinaryColor,
                                        Command = new Command
                                   (
                                       execute: async () =>
                                       {

                                       }
                                   )
                                    };
                                    layout.Add(delete);

                                    InvStack.Add(layout);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                DisplayAlert("Error!", eSql.Message, "OK");
                Debug.WriteLine("Exception: " + eSql.Message);
            }
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