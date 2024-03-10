using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DungeonSidekickMAUI;

public partial class MonsterCombat : ContentPage
{
    private Monster selectedMonster;
    private string dice;
	public MonsterCombat()
	{
		InitializeComponent();
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

        foreach (var monster in monsters)
        {
            HorizontalStackLayout layout = new HorizontalStackLayout();
            Label monsterLabel = new Label();
            monsterLabel.TextColor = (Color)fontColor;
            string name = monster.Name;
            monsterLabel.Text = name + " HP: " + monster.HP;
            layout.Add(monsterLabel);

            // Button that selects the monster to be fought in combat
            Button select = new Button
            {
                CommandParameter = monster,
                TextColor = fontColor,
                Text = "Select",
                BackgroundColor = TrinaryColor,
            };
            select.Clicked += SelectButton;
            layout.Add(select);
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

    private void PullDiceValue()
    {


        int WeaponID = Preferences.Default.Get("SelectedWeapon", -1);
        if (WeaponID == -1) return;
        string query = "SELECT damageDice FROM dbo.Weapon" +
            " WHERE WeaponID = @Id;";
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
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
                        cmd.Parameters.AddWithValue("@Id", WeaponID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dice = reader.GetString(0);
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

    private async void CombatPopup()
    {
        // Allows us to use the dynamic colors with the out object
        var hasValue = Application.Current.Resources.TryGetValue("FontC", out object fontColor);
        var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object frameColor);

        bool isPositive = true;
        Label entryLabel = new Label();
        entryLabel.TextColor = (Color)fontColor;

        // Create entry for number input
        var numberEntry = new Entry
        {
            Placeholder = "0",
            Keyboard = Keyboard.Numeric,
            WidthRequest = 200,
            TextColor = (Color)fontColor,
            BackgroundColor = (Color)frameColor
        };

        // Create button for if the modifier is positive or negative
        var plusOrMinus = new Button
        {
            Text = "+",
            WidthRequest = 50,
            TextColor = (Color)fontColor,
            BackgroundColor = (Color)frameColor
        };

        // Create button for submission
        var rollDice = new Button
        {
            Text = "Roll Dice",
            WidthRequest = 350,
            TextColor = (Color)fontColor,
            BackgroundColor = (Color)frameColor
        };

        // Create layout for making it look pretty
        var horizontalLayout = new StackLayout
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
        horizontalLayout.Children.Add(numberEntry);
        horizontalLayout.Children.Add(plusOrMinus);
        layout.Children.Add(horizontalLayout);
        layout.Children.Add(rollDice);

        // Create the popup
        var popup = new Popup
        {
            Content = layout
        };

        plusOrMinus.Clicked += async (sender, e) =>
        {
            if (isPositive)
            {
                isPositive = false;
                plusOrMinus.Text = "-";
            }
            else
            {
                isPositive = true;
                plusOrMinus.Text = "+";
            }
        };

        // Subscribe to button click event
        rollDice.Clicked += async (sender, e) =>
        {
            // Retrieve input number
            if (int.TryParse(numberEntry.Text, out int number))
            {
                PullDiceValue();
                await DisplayAlert("Dice", dice, "OK");
                DiceRoll roller = new DiceRoll();
                int result = roller.Roll(dice);
                MonsterSelector.Instance.DamageMonster(selectedMonster.Name, result);
                await DisplayAlert("Damage Dealt", "You dealt " + result + " damage, the monster has " + selectedMonster.HP + " remaining", "OK");
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid number", "OK");
            }

            // Close the popup
            popup.Close();
        };

        // Show the popup
        this.ShowPopup(popup);

        // Wait for the button click and return the entered number
    }

}