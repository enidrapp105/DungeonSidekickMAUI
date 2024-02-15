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
                    StackLayout RaceStack = new StackLayout();
                    var hasValue = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("FontC", out object fontColor);
                    var hasValue2 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("TrinaryColor", out object frameColor);
                    var hasValue3 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("SecondaryColor", out object headerColor);
                    var hasValue4 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("PrimaryColor", out object backgroundColor);
                    RaceStack.BackgroundColor = (Color)backgroundColor;

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
                                raceName = reader.GetString(0);
                                Race.Text = raceName;

                                Frame frame = new Frame()
                                {
                                    BackgroundColor = (Color)headerColor,
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
                            
                        }

                        BoxView line = new BoxView
                        {
                            Color = (Color)fontColor,
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        RaceStack.Children.Add(line);
                        query = "SELECT Bonus, AbilityName FROM dbo.AbilityBonuses" +
                                " WHERE RaceID = @RaceID2;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@RaceID2", selectedRace);
                        using (SqlDataReader reader2 = cmd.ExecuteReader())
                        {
                            Label Bonus = new Label();
                            Bonus.TextColor = (Color)fontColor;
                            Bonus.Text = "Bonuses: ";
                            RaceStack.Children.Add(Bonus);
                            while (reader2.Read())
                            {
                                Label AbilityName = new Label();
                                AbilityName.TextColor = (Color)fontColor;
                                AbilityName.Text = reader2.GetString(1) + " " + reader2.GetInt32(0);
                                RaceStack.Children.Add(AbilityName);
                            }
                        }

                        BoxView line2 = new BoxView
                        {
                            Color = (Color)fontColor,
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        RaceStack.Children.Add(line2);
                        query = "SELECT Bonus, AbilityName, Choice FROM dbo.AbilityBonusOptions" +
                            " WHERE RaceID = @RaceID3;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@RaceID3", selectedRace);
                        using (SqlDataReader reader3 = cmd.ExecuteReader())
                        {
                            Label Bonus = new Label();
                            Bonus.TextColor = (Color)fontColor;
                            Bonus.Text = "Choose Optional Bonuses: ";
                            RaceStack.Children.Add(Bonus);
                            int choice = 0;
                            while (reader3.Read())
                            {
                                Label AbilityName = new Label();
                                AbilityName.TextColor = (Color)fontColor;
                                AbilityName.Text = reader3.GetString(1) + " " + reader3.GetInt32(0);
                                RaceStack.Children.Add(AbilityName);
                                choice = reader3.GetInt32(2);
                            }
                            Label Choice = new Label();
                            Choice.TextColor = (Color)fontColor;
                            Choice.Text = "Choose " + choice;
                            RaceStack.Children.Add(Choice);
                        }

                        BoxView line3 = new BoxView
                        {
                            Color = (Color)fontColor,
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        RaceStack.Children.Add(line3);
                        query = "SELECT StartProfName FROM dbo.StartingProficiencies" +
                                " WHERE RaceID = @RaceID4;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@RaceID4", selectedRace);
                        using (SqlDataReader reader4 = cmd.ExecuteReader())
                        {
                            Label StartProf = new Label();
                            StartProf.TextColor = (Color)fontColor;
                            StartProf.Text = "Starting Proficiencies: ";
                            RaceStack.Children.Add(StartProf);
                            while (reader4.Read())
                            {
                                Label ProfName = new Label();
                                ProfName.TextColor = (Color)fontColor;
                                ProfName.Text = reader4.GetString(0);
                                RaceStack.Children.Add(ProfName);
                            }
                        }

                        BoxView line4 = new BoxView
                        {
                            Color = (Color)fontColor,
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        RaceStack.Children.Add(line4);
                        query = "SELECT StartProfOptName, Choice FROM dbo.StartingProficienciesOptions" +
                                " WHERE RaceID = @RaceID5;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@RaceID5", selectedRace);
                        using (SqlDataReader reader5 = cmd.ExecuteReader())
                        {
                            Label StartProf = new Label();
                            StartProf.TextColor = (Color)fontColor;
                            StartProf.Text = "Choose Optional Starting Proficiencies: ";
                            RaceStack.Children.Add(StartProf);
                            int choice = 0;
                            while (reader5.Read())
                            {
                                Label ProfName = new Label();
                                ProfName.TextColor = (Color)fontColor;
                                ProfName.Text = reader5.GetString(0);
                                RaceStack.Children.Add(ProfName);
                                choice = reader5.GetInt32(1);
                            }
                            Label Choice = new Label();
                            Choice.TextColor = (Color)fontColor;
                            Choice.Text = "Choose " + choice;
                            RaceStack.Children.Add(Choice);
                        }
                    }
                    Button submit = new Button()
                    {
                        BackgroundColor = (Color)frameColor,
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
        characterSheet.race = raceName;
        Navigation.PushAsync(new CSheet(characterSheet));
    }
}