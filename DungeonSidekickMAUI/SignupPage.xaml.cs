using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
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
        else if (!usernameCheck(usernamebox.Text))
        {
            DisplayAlert("Username already exists.", "Please try another username.", "Ok");
            validlogin = false;
        }
        if (validlogin) 
		{
			Password_Hasher password_Hasher = new Password_Hasher(usernamebox.Text);
			password_Hasher.HashPassword(passwordbox.Text); //HashPassword should handle the saving itself therefore removing the db need here
            Navigation.PushAsync(new LoginPage());
        }
    }
	private bool usernameCheck(string username) 
	{
        Connection connection = Connection.connectionSingleton;
        string query = "SELECT Username FROM dbo.Users WHERE Username = @Username COLLATE SQL_Latin1_General_CP1_CI_AS";
        try
        {
            using (SqlConnection conn = new SqlConnection(connection.connectionString))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                // If there are rows, it means the username is found
                                return false;
                            }
                        }
                    }
                }
                conn.Close();
            }
        }
        catch (Exception eSql)
        {
            DisplayAlert("Error!", eSql.Message, "OK");
        }
        return true;
	}
}