namespace DungeonSidekickMAUI;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private void loginButtonClicked(object sender, EventArgs e)
	{
		//put the salt code and data base stuff here


		//be sure to check if they are in the database before navigating
        Navigation.PushAsync(new MainPage());
    }
	private void signupButtonClicked(object sender, EventArgs e) 
	{
		Navigation.PushAsync(new SignupPage());
	}
    private void DebugbuttonButtonClicked(object sender, EventArgs e)
    {
        
        Navigation.PushAsync(new MainPage());
    }
}