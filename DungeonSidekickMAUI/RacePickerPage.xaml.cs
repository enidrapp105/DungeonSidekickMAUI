using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace DungeonSidekickMAUI;

public partial class RacePickerPage : ContentPage
{
    CharacterSheet characterSheet;
    public RacePickerPage(CharacterSheet characterSheet)
    {
        InitializeComponent();
        this.characterSheet = characterSheet;
        RaceButtonContainer = new StackLayout()
        {
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            VerticalOptions = LayoutOptions.StartAndExpand
        };
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        string query = "SELECT RaceID, Race FROM dbo.RaceLookup";
        RaceButtonContainer = this.FindByName<StackLayout>("RaceButtonContainer");
        Color color = new Color(255, 0, 0);
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
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetInt32(0);
                                var RaceButton = new Button
                                {
                                    Text = reader.GetString(1),
                                    CommandParameter = id,
                                    FontSize = 12,
                                    HeightRequest = 50,
                                    WidthRequest = 100,
                                    MinimumHeightRequest = 50,
                                    MinimumWidthRequest = 50,
                                    BackgroundColor = color

                                };
                                RaceButton.Clicked += OnRaceButtonClicked;

                                RaceButtonContainer.Children.Add(RaceButton);
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

        //this.Classpagestack.Children.Add(classButtonContainer);
    }
    /*
     * Function: RollForStats
     * Author: Kenny Rapp
     * Purpose: Navigate to the ClassPicker
     * last Modified : 12/04/2023 3:20pm
     */
    private void OnRaceButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button RaceButton && RaceButton.CommandParameter is int id)
        {
            Navigation.PushAsync(new SelectedRacePage(characterSheet, id));
        }
    }
}
