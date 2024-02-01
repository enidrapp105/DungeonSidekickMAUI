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
		if(usernamebox.Text == null || usernamebox.Text.Length < 8)
		{
            DisplayAlert("Usernames must be at least 8 characters", "Please fix your username", "Ok");
            validlogin = false;
		}
		if(passwordbox.Text != passwordbox2.Text)
		{
			DisplayAlert("Passwords must match", "Please fix your passwords", "Ok");
			validlogin = false;
		}
		if(passwordbox.Text == null || passwordbox2.Text == null || passwordbox.Text.Length < 8 || passwordbox2.Text.Length < 8) 
		{
			DisplayAlert("Passwords must be at least 8 characters", "Please fix your passwords", "Ok");
			validlogin = false;
        }
		if(validlogin) 
		{
			Password_Hasher password_Hasher = new Password_Hasher(usernamebox.Text);
			password_Hasher.HashPassword(passwordbox.Text); //HashPassword should handle the saving itself therefore removing the db need here
            Navigation.PushAsync(new LoginPage());
        }
    }
}