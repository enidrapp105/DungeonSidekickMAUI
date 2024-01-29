using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui;
using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.Maui;
namespace DungeonSidekickMAUI;

public partial class SelectedRacePage : ContentPage
{
    CharacterSheet characterSheet;
    string raceName;
    public SelectedRacePage(CharacterSheet characterSheet, int selectedRace)
    {
        this.characterSheet = characterSheet;
        InitializeComponent();
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

        string query = "SELECT Race, Description, MoveSpeed, Age, Size, SizeDescription, Languages, LanguageDescription FROM dbo.RaceLookup" +
            " WHERE RaceID = @RaceID;";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@RaceID", selectedRace);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StackLayout RaceStack = new StackLayout();
                                RaceStack.BackgroundColor = Color.FromRgb(0,0,0);
                                Label Race = new Label();
                                Race.FontSize = 36;
                                Race.HorizontalTextAlignment = TextAlignment.Center;
                                Race.TextColor = Color.FromRgb(255, 255, 255);
                                raceName = reader.GetString(0);
                                Race.Text = "Race: " + raceName;
                                

                                Label Speed = new Label();
                                Race.TextColor = Color.FromRgb(255, 255, 255);
                                Race.Text = "Move Speed: " + reader.GetInt32(2);

                                Label Desc = new Label();
                                Desc.TextColor = Color.FromRgb(255, 255, 255);
                                Desc.Text = reader.GetString(1);

                                Label Age = new Label();
                                Age.TextColor = Color.FromRgb(255, 255, 255);
                                Age.Text = reader.GetString(3);

                                Label Size = new Label();
                                Size.TextColor = Color.FromRgb(255, 255, 255);
                                Size.Text = "Size: " + reader.GetString(4);

                                Label SizeDesc = new Label();
                                SizeDesc.TextColor = Color.FromRgb(255, 255, 255);
                                SizeDesc.Text = reader.GetString(5);

                                Label Lang = new Label();
                                Lang.TextColor = Color.FromRgb(255, 255, 255);
                                Lang.Text = "Languages: " + reader.GetString(6);

                                Label LangDesc = new Label();
                                LangDesc.TextColor = Color.FromRgb(255, 255, 255);
                                LangDesc.Text = reader.GetString(1);

                                RaceStack.Children.Add(Race);
                                RaceStack.Children.Add(Speed);
                                RaceStack.Children.Add(Desc);
                                RaceStack.Children.Add(Age);
                                RaceStack.Children.Add(Size);
                                RaceStack.Children.Add(SizeDesc);
                                RaceStack.Children.Add(Lang);
                                RaceStack.Children.Add(LangDesc);
                                MainPanel.Children.Add(RaceStack);
                            }
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
    }

    private void Submit(object sender, EventArgs e)
    {
        characterSheet.race = raceName;
        Navigation.PushAsync(new CSheet(characterSheet));
    }
}