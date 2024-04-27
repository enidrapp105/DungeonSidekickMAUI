namespace DungeonSidekickMAUI;

public partial class Info_For_Stats : ContentPage
{
    public Info_For_Stats()
    {
        InitializeComponent();

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, true);
        ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());
    }

    private void To_CSheet(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CSheet());
    }
}