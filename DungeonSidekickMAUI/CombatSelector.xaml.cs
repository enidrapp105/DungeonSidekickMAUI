namespace DungeonSidekickMAUI;

public partial class CombatSelector : ContentPage
{
	public CombatSelector()
	{
		InitializeComponent();
	}

    private void NavigateToPCombat(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CombatPage());
    }

    private void NavigateToSCombat(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SpellCombatPage());
    }
}