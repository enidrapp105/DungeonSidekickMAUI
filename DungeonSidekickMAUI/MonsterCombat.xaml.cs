namespace DungeonSidekickMAUI;

public partial class MonsterCombat : ContentPage
{
    private Monster selectedMonster;
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

            // Button that selects the item to be used in combat
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
                await DisplayAlert("Selected Monster", "Successfully selected the monster for combat.", "Ok");
        }
    }

}