namespace DungeonSidekickMAUI;

public partial class Info_For_Stats : ContentPage
{
    // added this bool to keep track of if the account is brand new, if it has no saved characters then the nav bar doesn't appear
    bool m_NewAcc;
    public Info_For_Stats(bool newAcc = false)
    {
        m_NewAcc = newAcc;
        InitializeComponent();

        // nav bar setup
        if (!m_NewAcc)
        {
            Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
            NavigationCommands cmd = new NavigationCommands();
            NavigationPage.SetHasNavigationBar(this, true);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
            NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());
        }
    }

    private void To_CSheet(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CSheet(m_NewAcc));
    }
}