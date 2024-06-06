//using UIKit;

namespace DungeonSidekickMAUI;

public partial class CombatSelector : ContentPage
{
	public CombatSelector()
	{
		InitializeComponent();

        // nav bar setup
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, false);
        var customNavBar = cmd.CreateCustomNavigationBar();
        NavigationBar.Children.Add(customNavBar);
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