using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

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
        List<string> list = new List<string>();
        list.Add(UName.Text);
        list.Add(Pass.Text);
        if (!GlobalFunctions.entryCheck(list, 0))
            return;
		if (password_Hasher.VerifyHashedPassword(Pass.Text))
		{

            if (Character_Count() > 0) 
            {
                Navigation.PushAsync(new CSheet_Import());
            }
            else
            {
                Navigation.PushAsync(new Info_For_Stats());
            }
            //Navigation.PushAsync(new MainPage());
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

    private int Character_Count()
    {
        int result = 0;
        Connection connection = Connection.connectionSingleton;
		string query = "Select CharacterID FROM dbo.CharacterSheet WHERE UID = @UID;";
        int UserId = Preferences.Default.Get("UserId", -1);

        try
        {
            using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.Add("@UID", SqlDbType.Int).Value = UserId;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result++;
                        }
                    }
                }
            }
        }
        catch (Exception eSql)
        {
            DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
        }
        return result;
        App.Current.MainPage = new AppShell();
    }
  
}