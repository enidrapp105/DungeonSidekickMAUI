using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace DungeonSidekickMAUI;

public partial class RacePickerPage : ContentPage
{
    ImportedCharacterSheet characterSheet;
    private bool m_NewAcc;
    private bool m_ModSheet;
    public RacePickerPage(bool modSheet, ImportedCharacterSheet characterSheet, bool newAcc = false)
    {
        InitializeComponent();
        m_ModSheet = modSheet;
        // nav bar setup
        m_NewAcc = newAcc;
        if (!m_NewAcc)
        {
            NavigationCommands cmd = new NavigationCommands();
            NavigationPage.SetHasNavigationBar(this, false);
            var customNavBar = cmd.CreateCustomNavigationBar();
            NavigationBar.Children.Add(customNavBar);
        }

        this.characterSheet = characterSheet;
        RaceButtonContainer = new StackLayout()
        {
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            VerticalOptions = LayoutOptions.StartAndExpand
        };
        Connection connection = Connection.connectionSingleton;
        string query = "SELECT RaceID, Race FROM dbo.RaceLookup";
        RaceButtonContainer = this.FindByName<StackLayout>("RaceButtonContainer");
        Color color = new Color(255, 0, 0);
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
                        var hasValue = Application.Current.Resources.TryGetValue("FontC", out object fontColor);
                        var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object frameColor);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetInt32(0);
                                var RaceButton = new Button
                                {
                                    Text = reader.GetString(1),
                                    TextColor = (Color)fontColor,
                                    CommandParameter = id,
                                    HeightRequest = 50,
                                    WidthRequest = 400,
                                    Margin = 5,
                                    MinimumHeightRequest = 50,
                                    MinimumWidthRequest = 50,
                                    BackgroundColor = (Color)frameColor

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
    }
    /*
     * Function: OnRaceButtonClicked
     * Author: Anthony Rielly
     * Purpose: Navigate to the SelectedRace
     * last Modified : 1/28/2024 6:00pm
     */
    private void OnRaceButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button RaceButton && RaceButton.CommandParameter is int id)
        {
            Navigation.PushAsync(new SelectedRacePage(m_ModSheet, characterSheet, id, m_NewAcc));
        }
    }
}
