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
		Password_Hasher password_Hasher = new Password_Hasher(UName.Text);
		//pull db for salted password here
		if(password_Hasher.VerifyHashedPassword(Pass.Text))
            Navigation.PushAsync(new MainPage());
		else
			DisplayAlert("Your username or password are incorrect", "Please try a different username or password", "Ok");
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