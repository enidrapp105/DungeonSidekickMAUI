using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
namespace DungeonSidekickMAUI;

public partial class ClassPickerPage : ContentPage
{
    ImportedCharacterSheet characterSheet;
    private bool m_NewAcc;
    private bool m_ModSheet;
    /*
     * Function: ClassPicker default Constructor
     * Author: Kenny Rapp
     * Purpose: Initilizes all of the class choises based on the json file
     * last Modified : 02/04/2023 9:00pm
     * Modified By Anthony Rielly
     * Modifications: Removed json string and switched to creating the buttons from a DB lookup, rather than hard coded.
     */
    public ClassPickerPage(bool modSheet, ImportedCharacterSheet CharacterSheet,  bool newAcc = false)
    {
        InitializeComponent();
        m_ModSheet = modSheet;

        //nav bar setup
        m_NewAcc = newAcc;
        if (!m_NewAcc)
        {
            NavigationCommands cmd = new NavigationCommands();
            NavigationPage.SetHasNavigationBar(this, false);
            var customNavBar = cmd.CreateCustomNavigationBar();
            NavigationBar.Children.Add(customNavBar);
        }
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
                        var hasValue3 = Application.Current.Resources.TryGetValue("TrinaryColor", out object trinaryColor);
                                                    var hasValue4 = Application.Current.Resources.TryGetValue("PrimaryColor", out object primaryColor);
                        var hasValue5 = Application.Current.Resources.TryGetValue("AccentColor", out object accentColor);
                        var hasValue6 = Application.Current.Resources.TryGetValue("accessoryColor", out object accessoryColor);
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
                                    BackgroundColor = (Color)accentColor

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
            Navigation.PushAsync(new SelectedClassPage(m_ModSheet, characterSheet, selectedClass, m_NewAcc));
        }
    }
}