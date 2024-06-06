using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DungeonSidekickMAUI;

public partial class MonsterCombat : ContentPage
{
    private Monster selectedMonster;
    //private string dice;
	public MonsterCombat()
	{
		InitializeComponent();

        // nav bar setup
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, false);
        var customNavBar = cmd.CreateCustomNavigationBar();
        NavigationBar.Children.Add(customNavBar);

        DisplayAllMonsters();
	}

   /*
    * Function: DisplayAllMonsters
    * Author: Brendon Williams
    * Purpose: Grabs all the monsters the user selected in the last page, and then displays them all to be selected
    * last Modified : 3/10/2024 4:06pm
    */
    public void DisplayAllMonsters()
    {
        Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
        Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
        Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];

        List<Monster> monsters = MonsterSelector.Instance.m_monsters;
        Label title = new Label();
        title.FontSize = 36;
        title.Text = "Chosen Monsters";
        title.HorizontalTextAlignment = TextAlignment.Center;
        title.TextColor = fontColor;
        MonsterStack.Add(title);

        foreach (var monster in monsters)
        {
            HorizontalStackLayout layout = new HorizontalStackLayout();
            Label monsterLabel = new Label();
            monsterLabel.TextColor = (Color)fontColor;
            string name = monster.Name;
            monsterLabel.Text = name + " HP: " + monster.HP;


            // Button that selects the monster to be fought in combat
            Button select = new Button
            {
                CommandParameter = monster,
                TextColor = fontColor,
                Text = "Select",
                BackgroundColor = TrinaryColor,
                Margin = new Thickness(10,0,10,10)
            };
            select.Clicked += SelectButton;
            layout.Add(select);
            layout.Add(monsterLabel);
            MonsterStack.Add(layout);
        }
    }

    /*
    * Function: SelectButton
    * Author: Brendon Williams
    * Purpose: It makes the selected monster the one the user hits select on last (kinda duh)
    * last Modified : 3/10/2024 4:07pm
    */
    private async void SelectButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Monster monster)
        {
            selectedMonster = monster;
            CombatPopup();
            //await DisplayAlert("Selected Monster", "Successfully selected the monster for combat.", "Ok");
        }
    }

    private void PullDiceValue() // Leaving this in because I don't want to delete someone else's work without permission.
    {
        ImportedCharacterSheet character = ImportedCharacterSheet.Instance;
        //int WeaponID = Preferences.Default.Get("SelectedWeapon", -1);
        int WeaponID = character.c_WEquippedID; // Shouldn't need this, just wanted to add it because functionality changed.
        if (WeaponID == -1) return;
        character.EquipItem(WeaponID, 0);
        //dice = character.damageDice;
        //string query = "SELECT damageDice FROM dbo.Weapon" +
        //    " WHERE WeaponID = @Id;";
        //Connection connection = Connection.connectionSingleton;
        //try
        //{
        //    using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
        //    {
        //        conn.Open();
        //        if (conn.State == System.Data.ConnectionState.Open)
        //        {
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = query;

        //                // grabs ID from weapon list
        //                cmd.Parameters.AddWithValue("@Id", WeaponID);
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        dice = reader.GetString(0);
        //                        character.damageDice = reader.GetString(0); // Testing if equipping works.
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //catch (Exception eSql)
        //{
        //    DisplayAlert("Error!", eSql.Message, "OK");
        //    Debug.WriteLine("Exception: " + eSql.Message);
        //}
    }


    private async void CombatPopup()
    {
        // Allows us to use the dynamic colors with the out object
        Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
        Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
        Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];

        ImportedCharacterSheet character = ImportedCharacterSheet.Instance;

        bool hitIsPositive = true;
        bool dmgIsPositive = true;
        Label entryLabel = new Label();
        entryLabel.TextColor = (Color)fontColor;

        // Create entry for number input
        var dmgEntry = new Entry
        {
            Placeholder = "Damage modifier",
            Keyboard = Keyboard.Numeric,
            WidthRequest = 250,
            MaxLength = 5,
            TextColor = fontColor,
            BackgroundColor = SecondaryColor
        };

        // Create button for if the modifier is positive or negative
        var dmgPlusOrMinus = new Button
        {
            Text = "+",
            WidthRequest = 100,
            TextColor = fontColor,
            BackgroundColor = SecondaryColor
        };

        // Create entry for number input
        var hitEntry = new Entry
        {
            Placeholder = "Hit modifier",
            Keyboard = Keyboard.Numeric,
            WidthRequest = 250,
            MaxLength = 5,
            TextColor = fontColor,
            BackgroundColor = SecondaryColor
        };

        // Create button for if the modifier is positive or negative
        var hitPlusOrMinus = new Button
        {
            Text = "+",
            WidthRequest = 100,
            TextColor = fontColor,
            BackgroundColor = SecondaryColor
        };

        // Create button for submission
        var rollDice = new Button
        {
            Text = "Roll Dice",
            WidthRequest = 350,
            TextColor = fontColor,
            BackgroundColor = SecondaryColor
        };

        // Create layout for making it look pretty
        var dmgHorizontalLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center
        };

        // Create layout for making it look pretty
        var hitHorizontalLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center
        };

        // Create layout for popup contents
        var layout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Center
        };
        layout.Children.Add(entryLabel);
        hitHorizontalLayout.Children.Add(hitEntry);
        hitHorizontalLayout.Children.Add(hitPlusOrMinus);
        dmgHorizontalLayout.Children.Add(dmgEntry);
        dmgHorizontalLayout.Children.Add(dmgPlusOrMinus);
        layout.Children.Add(hitHorizontalLayout);
        layout.Children.Add(dmgHorizontalLayout);
        layout.Children.Add(rollDice);

        // Create the popup
        var popup = new Popup
        {
            Content = layout
        };

        hitPlusOrMinus.Clicked += async (sender, e) =>
        {
            if (hitIsPositive)
            {
                hitIsPositive = false;
                hitPlusOrMinus.Text = "-";
            }
            else
            {
                hitIsPositive = true;
                hitPlusOrMinus.Text = "+";
            }
        };

        dmgPlusOrMinus.Clicked += async (sender, e) =>
        {
            if (dmgIsPositive)
            {
                dmgIsPositive = false;
                dmgPlusOrMinus.Text = "-";
            }
            else
            {
                dmgIsPositive = true;
                dmgPlusOrMinus.Text = "+";
            }
        };

        // Subscribe to button click event
        rollDice.Clicked += async (sender, e) =>
        {
            // Retrieve input number
            int dmgMod = 0;
            int hitMod = 0;

            if (int.TryParse(dmgEntry.Text, out int num))
            {
                dmgMod = num;
            }

            if (int.TryParse(hitEntry.Text, out int num2))
            {
                hitMod = num2;
            }

            if (!hitIsPositive)
            {
                hitMod *= -1;
            }
            
            //PullDiceValue(); // Figured out why this was being called.
            //await DisplayAlert("Dice", dice, "OK");
            DiceRoll roller = new DiceRoll();
            int acRoll = roller.Roll("1d20");
            bool throughAC = false;
            int total = acRoll + hitMod;
            if (acRoll == 20)
            {
                throughAC = true;
                await DisplayAlert("Hit Roll", "You rolled a natural " + acRoll + " and got past the monsters AC", "OK");
            }
            else if (acRoll == 1)
            {
                throughAC = false;
                await DisplayAlert("Hit Roll", "You rolled a " + acRoll + " and failed to hit the monster", "OK");
            }
            else if (total > selectedMonster.AC)
            {
                throughAC = true;
                await DisplayAlert("Hit Roll", "You rolled a " + total + " and got past the monsters AC of " + selectedMonster.AC, "OK");
            }
            else
            {
                throughAC = false;
                await DisplayAlert("Hit Roll", "You rolled a " + total + " and fail to get past the monsters AC of " + selectedMonster.AC, "OK");
            }
            if (throughAC)
            {
                int result = roller.Roll(character.c_damageDice);
                if (dmgIsPositive)
                {
                    result += dmgMod;
                }
                else
                {
                    result -= dmgMod;
                }
                MonsterSelector.Instance.DamageMonster(selectedMonster.Name, result);
                await DisplayAlert("Damage Dealt", "You dealt " + result + " damage, the monster has " + selectedMonster.HP + " remaining", "OK");
            }

            // Close the popup
            popup.Close();

            // clear the stack and re create it to update the HP values
            MonsterStack.Clear();
            DisplayAllMonsters();
        };

        // Show the popup
        this.ShowPopup(popup);

    }

}