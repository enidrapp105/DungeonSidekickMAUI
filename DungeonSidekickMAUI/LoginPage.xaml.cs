namespace DungeonSidekickMAUI;
 

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}


    private void loginButtonClicked(object sender, EventArgs e)
	{
		Password_Hasher password_Hasher = new Password_Hasher(UName.Text);
		//checks the db and returns true if check passes.
		//Also updates Preferences.Default.Get "Username" and "UserId" to the correct values
		//For reference as to how they are called. string username = Preferences.Default.Get("Username", "");
		//Where the second quote is the default value if the preference isn't set
		if (password_Hasher.VerifyHashedPassword(Pass.Text))
		{
            //because the login page is outside of the appshell we must create an appshell based on the mainpage
            App.Current.MainPage = new AppShell();
        }
		else
		{
			DisplayAlert("Your username or password are incorrect", "Please try a different username or password", "Ok");
		}
        
        
    }
	private void signupButtonClicked(object sender, EventArgs e) 
	{
		Navigation.PushAsync(new SignupPage());
	}
    private void DebugbuttonButtonClicked(object sender, EventArgs e)
    {
		Preferences.Default.Set("Username", "Admin");
		Preferences.Default.Set("UserId", 1);
		//because the login page is outside of the appshell we must create an appshell based on the mainpage
        App.Current.MainPage = new AppShell();
    }
  
}