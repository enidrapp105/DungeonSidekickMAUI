namespace DungeonSidekickMAUI;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private void loginButtonClicked(object sender, EventArgs e)
	{
        Navigation.PushAsync(new MainPage());
    }

}