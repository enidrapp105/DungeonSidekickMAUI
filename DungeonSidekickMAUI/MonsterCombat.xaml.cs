namespace DungeonSidekickMAUI;

public partial class MonsterCombat : ContentPage
{
    private Monster selectedMonster;
	public MonsterCombat()
	{
		InitializeComponent();
		DisplayAllMonsters();
	}

    public void DisplayAllMonsters()
    {
        Color PrimaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        Color SecondaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["SecondaryColor"];
        Color TrinaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["TrinaryColor"];
        Color fontColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["FontC"];

        HorizontalStackLayout layout = new HorizontalStackLayout();
        
        List<Monster> monsters = MonsterSelector.Instance.m_monsters;

        foreach (var monster in monsters)
        {
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

    private async void SelectButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Monster monster)
        {
                selectedMonster = monster;
                await DisplayAlert("Selected Monster", "Successfully selected the monster for combat.", "Ok");
        }
    }

}