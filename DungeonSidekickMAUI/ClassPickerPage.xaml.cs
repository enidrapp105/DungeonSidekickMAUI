using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;

public partial class ClassPickerPage : ContentPage
{
    CharacterSheet characterSheet;
    
    /*
     * Function: ClassPicker default Constructor
     * Author: Kenny Rapp
     * Purpose: Initilizes all of the class choises based on the json file
     * last Modified : 02/04/2023 9:00pm
     * Modified By Anthony Rielly
     * Modifications: Removed json string and switched to creating the buttons from a DB lookup, rather than hard coded.
     */
    public ClassPickerPage(CharacterSheet CharacterSheet)
    {
        InitializeComponent();

        //nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands nav = new NavigationCommands();
        Microsoft.Maui.Controls.NavigationPage.SetHasNavigationBar(this, true);
        ((Microsoft.Maui.Controls.NavigationPage)Microsoft.Maui.Controls.Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        Microsoft.Maui.Controls.NavigationPage.SetTitleView(this, nav.CreateCustomNavigationBar());

        this.characterSheet = CharacterSheet;
        ClassButtonContainer = new StackLayout()
        {
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            VerticalOptions = LayoutOptions.StartAndExpand
        };

        Connection connection = Connection.connectionSingleton;
        string query = "SELECT ClassID, Class FROM dbo.ClassLookup";
        ClassButtonContainer = this.FindByName<StackLayout>("ClassButtonContainer");
        Color color = new Color(255, 0, 0);
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
                        var hasValue = Application.Current.Resources.TryGetValue("FontC", out object fontColor);
                        var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object frameColor);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetInt32(0);

                                // Creates a button for each of the classes found in the DB
                                var ClassButton = new Button
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
                                ClassButton.Clicked += OnClassButtonClicked;

                                ClassButtonContainer.Children.Add(ClassButton);
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
     * Function: OnClassButtonClicked
     * Author: Kenny Rapp
     * Purpose: Navigate to the ClassPicker
     * last Modified : 12/04/2023 3:20pm
     */
    private void OnClassButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button classButton && classButton.CommandParameter is int selectedClass)
        {
            Navigation.PushAsync(new SelectedClassPage(characterSheet, selectedClass));
        }
    }
}