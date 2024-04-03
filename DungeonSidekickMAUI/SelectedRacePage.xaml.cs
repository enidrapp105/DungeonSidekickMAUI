using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui;
using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.Maui;
using System.Runtime.CompilerServices;
namespace DungeonSidekickMAUI;

public partial class SelectedRacePage : ContentPage
{
    int RaceID;
    CharacterSheet characterSheet;
    int raceId;
    public SelectedRacePage(CharacterSheet characterSheet, int selectedRace)
    {
        this.characterSheet = characterSheet;
        InitializeComponent();
        Connection connection = Connection.connectionSingleton;
        RaceID = selectedRace;

        string query = "SELECT Race, Description, MoveSpeed, Age, Size, SizeDescription, Languages, LanguageDescription FROM dbo.RaceLookup" +
            " WHERE RaceID = @RaceID;";
        var hasValue = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("FontC", out object fontColor);
        var hasValue2 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("SecondaryColor", out object secondaryColor);
        var hasValue3 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("TrinaryColor", out object trinaryColor);
        var hasValue4 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("PrimaryColor", out object primaryColor);
        try
        {
            using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    StackLayout RaceStack = new StackLayout();

                    
                    RaceStack.BackgroundColor = (Color)primaryColor;
                    raceId = selectedRace;

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@RaceID", selectedRace);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Label Race = new Label();
                                Race.FontSize = 36;
                                Race.HorizontalTextAlignment = TextAlignment.Center;

                                Race.TextColor = (Color)fontColor;
                                string raceName = reader.GetString(0);
                                characterSheet.raceName = raceName;
                                Race.Text = raceName;

                                Frame frame = new Frame()
                                {
                                    BackgroundColor = (Color)trinaryColor,
                                    Padding = 24,
                                    CornerRadius = 0,
                                    Content = Race
                                };

                                Label Speed = new Label();
                                Speed.TextColor = (Color)fontColor;
                                Speed.Text = "Move Speed: " + reader.GetInt32(2);

                                Label Desc = new Label();
                                Desc.TextColor = (Color)fontColor;
                                Desc.Text = reader.GetString(1);

                                Label Age = new Label();
                                Age.TextColor = (Color)fontColor;
                                Age.Text = reader.GetString(3);

                                Label Size = new Label();
                                Size.TextColor = (Color)fontColor;
                                Size.Text = "Size: " + reader.GetString(4);

                                Label SizeDesc = new Label();
                                SizeDesc.TextColor = (Color)fontColor;
                                SizeDesc.Text = reader.GetString(5);

                                Label Lang = new Label();
                                Lang.TextColor = (Color)fontColor;
                                Lang.Text = "Languages: " + reader.GetString(6);

                                Label LangDesc = new Label();
                                LangDesc.TextColor = (Color)fontColor;
                                LangDesc.Text = reader.GetString(1);

                                RaceStack.Children.Add(frame);
                                RaceStack.Children.Add(Speed);
                                RaceStack.Children.Add(Desc);
                                RaceStack.Children.Add(Age);
                                RaceStack.Children.Add(Size);
                                RaceStack.Children.Add(SizeDesc);
                                RaceStack.Children.Add(Lang);
                                RaceStack.Children.Add(LangDesc);
                            }
                            reader.Close();
                        }

                        query = "SELECT ProfID, Optional, Choice FROM dbo.RaceProficienciesLookup" +
                                    " WHERE RaceID = @RaceID2;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@RaceID2", selectedRace);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int newOption = 0;
                            while (reader.Read())
                            {
                                int Id = reader.GetInt32(0);
                                int optional = reader.GetInt32(1);
                                int choice = reader.GetInt32(2);
                                if (optional != newOption && optional != 0)
                                {
                                    if (optional == 1)
                                    {
                                        Label StartProf = new Label();
                                        StartProf.TextColor = (Color)fontColor;
                                        StartProf.Text = "Choose Optional Starting Proficiencies: ";
                                        RaceStack.Children.Add(StartProf);
                                    }
                                    Label Choice = new Label();
                                    Choice.TextColor = (Color)fontColor;
                                    Choice.Text = "Choose " + choice;
                                    RaceStack.Children.Add(Choice);
                                }
                                newOption = optional;

                                string innerQuery = "SELECT ProfName FROM dbo.ProficienciesLookup" +
                                " WHERE ProfID = @ProfID;";
                                try
                                {

                                    using (SqlConnection innerConn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
                                    {
                                        using (SqlCommand innerCmd = innerConn.CreateCommand())
                                        {
                                            innerCmd.CommandText = innerQuery;
                                            innerCmd.Parameters.AddWithValue("@ProfID", Id);
                                            innerConn.Open();
                                            if (innerConn.State == System.Data.ConnectionState.Open)
                                            {
                                                using (SqlDataReader innerReader = innerCmd.ExecuteReader())
                                                {
                                                    while (innerReader.Read())
                                                    {
                                                        Label ProfName = new Label();
                                                        ProfName.TextColor = (Color)fontColor;
                                                        ProfName.Text = innerReader.GetString(0);
                                                        RaceStack.Children.Add(ProfName);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception eSql)
                                {
                                    DisplayAlert("Error!", eSql.Message, "OK"); // Should be removed and replaced before final product
                                    Debug.WriteLine("Exception: " + eSql.Message);
                                }
                            }
                        }
                        query = "SELECT BonusID, Bonus, Optional, Choice FROM dbo.RaceBonusLookup" +
                                " WHERE RaceID = @RaceID3;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@RaceID3", selectedRace);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            
                            int newOption = 0;
                            while (reader.Read())
                            {
                                int Id = reader.GetInt32(0);
                                int bonus = reader.GetInt32(1);
                                int optional = reader.GetInt32(2);
                                int choice = reader.GetInt32(3);
                                if (optional != newOption && optional != 0 && choice != 0)
                                {
                                    if (optional == 1)
                                    {
                                        Label StartProf = new Label();
                                        StartProf.TextColor = (Color)fontColor;
                                        StartProf.Text = "Choose Optional Ability Bonuses: ";
                                        RaceStack.Children.Add(StartProf);
                                    }
                                    Label Choice = new Label();
                                    Choice.TextColor = (Color)fontColor;
                                    Choice.Text = "Choose " + choice;
                                    RaceStack.Children.Add(Choice);
                                }
                                newOption = optional;

                                string innerQuery = "SELECT BonusName FROM dbo.AbilityBonusLookup" +
                                " WHERE BonusID = @BonusID;";
                                try
                                {

                                    using (SqlConnection innerConn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
                                    {
                                        using (SqlCommand innerCmd = innerConn.CreateCommand())
                                        {
                                            innerCmd.CommandText = innerQuery;
                                            innerCmd.Parameters.AddWithValue("@BonusID", Id);
                                            innerConn.Open();
                                            if (innerConn.State == System.Data.ConnectionState.Open)
                                            {
                                                using (SqlDataReader innerReader = innerCmd.ExecuteReader())
                                                {
                                                    while (innerReader.Read())
                                                    {
                                                        Label BonusName = new Label();
                                                        BonusName.TextColor = (Color)fontColor;
                                                        BonusName.Text = innerReader.GetString(0) + " " + bonus;
                                                        RaceStack.Children.Add(BonusName);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception eSql)
                                {
                                    DisplayAlert("Error!", eSql.Message, "OK"); // Should be removed and replaced before final product
                                    Debug.WriteLine("Exception: " + eSql.Message);
                                }
                            }
                        }
                    }

                    Button submit = new Button()
                    {
                        BackgroundColor = (Color)secondaryColor,
                        TextColor = (Color)fontColor,
                        Text = "Submit"
                    };
                    submit.Clicked += Submit;
                    RaceStack.Children.Add(submit);
                    MainPanel.Children.Add(RaceStack);
                }
            }
        }
        catch (Exception eSql)
        {
            DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
        }
    }

    private void Submit(object sender, EventArgs e)
    {
        characterSheet.race = raceId;
        Navigation.PushAsync(new CSheet());
    }
}