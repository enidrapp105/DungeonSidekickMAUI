namespace DungeonSidekickMAUI;

public partial class CombatSelector : ContentPage
{
	public CombatSelector()
	{
		InitializeComponent();

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, true);
        ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());
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