using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace DungeonSidekickMAUI;

public partial class SignupPage : ContentPage
{
	public SignupPage()
	{
		InitializeComponent();
	}

    private void signupButtonClicked(object sender, EventArgs e)
	{
		bool validlogin = true;
		List<string> list = new List<string>();
		list.Add(usernamebox.Text);
		list.Add(passwordbox.Text);
		list.Add(passwordbox2.Text);
        if (!GlobalFunctions.entryCheck(list, 0))
            return;
        if (usernamebox.Text == null || usernamebox.Text.Length < 8)
		{
            DisplayAlert("Usernames must be at least 8 characters", "Please fix your username", "Ok");
            validlogin = false;
		}
        else if (usernamebox.Text.Length > 32)
        {
            DisplayAlert("Usernames must be less than 32 characters", "Please fix your username", "Ok");
            validlogin = false;
        }
        if (passwordbox.Text != passwordbox2.Text)
		{
			DisplayAlert("Passwords must match", "Please fix your passwords", "Ok");
			validlogin = false;
		}
		if(passwordbox.Text == null || passwordbox2.Text == null || passwordbox.Text.Length < 8 || passwordbox2.Text.Length < 8) 
		{
			DisplayAlert("Passwords must be at least 8 characters", "Please fix your passwords", "Ok");
			validlogin = false;
        }
        else if (passwordbox.Text.Length > 60 || passwordbox2.Text.Length > 60)
        {
            DisplayAlert("Passwords must be less than 60 characters", "Please fix your passwords", "Ok");
            validlogin = false;
        }
        if (validlogin) 
		{
			Password_Hasher password_Hasher = new Password_Hasher(usernamebox.Text);
			password_Hasher.HashPassword(passwordbox.Text); //HashPassword should handle the saving itself therefore removing the db need here
            Navigation.PushAsync(new LoginPage());
        }
    }
}