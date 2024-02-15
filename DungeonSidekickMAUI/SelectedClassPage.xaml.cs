using System.Diagnostics;
using Microsoft.Data.SqlClient;
//using Network;
namespace DungeonSidekickMAUI;

public partial class SelectedClassPage : ContentPage
{
    CharacterSheet characterSheet;
    string ClassName;
    /*
     * Function: SelectedClassPage Constructor
     * Author: Kenny Rapp
     * Purpose: Print out information about your selected class
     * last Modified : 02/04/2023 9:00pm
     * Modified By Anthony Rielly
     * Modifications: Pulls from the database lookup table to print out the information
     */
    public SelectedClassPage(CharacterSheet characterSheet, int selectedClass)
    {
        this.characterSheet = characterSheet;
        InitializeComponent();

        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        string query = "SELECT Class, HitDie FROM dbo.ClassLookup" +
            " WHERE ClassID = @ClassID;";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    StackLayout ClassStack = new StackLayout();
                    var hasValue = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("FontC", out object fontColor);
                    var hasValue2 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("FrameC", out object frameColor);
                    var hasValue3 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("HeaderC", out object headerColor);
                    var hasValue4 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("BackgroundC", out object backgroundColor);
                    ClassStack.BackgroundColor = (Color)backgroundColor;

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@ClassID", selectedClass);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Label Class = new Label();
                                Class.FontSize = 36;
                                Class.HorizontalTextAlignment = TextAlignment.Center;
                                Class.TextColor = (Color)fontColor;
                                ClassName = reader.GetString(0);
                                Class.Text = ClassName;

                                Frame frame = new Frame()
                                {
                                    BackgroundColor = (Color)headerColor,
                                    Padding = 24,
                                    CornerRadius = 0,
                                    Content = Class
                                };
                                Label HitDie = new Label();
                                HitDie.HorizontalTextAlignment = TextAlignment.Center;
                                HitDie.TextColor = (Color)fontColor;
                                HitDie.Text = "Hit Die: " + reader.GetInt32(1);
                                ClassStack.Children.Add(Class);
                                ClassStack.Children.Add(HitDie);
                            }
                            reader.Close(); // allows reader to be used again instead of creating reader2, 3, etc
                        }

                        query = "SELECT StartProfName FROM dbo.StartingProficiencies" +
                                " WHERE ClassID = @ClassID2;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@ClassID2", selectedClass);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Label StartProf = new Label();
                            StartProf.TextColor = (Color)fontColor;
                            StartProf.Text = "Starting Proficiencies: ";
                            ClassStack.Children.Add(StartProf);
                            while (reader.Read())
                            {
                                Label ProfName = new Label();
                                ProfName.TextColor = (Color)fontColor;
                                ProfName.Text = reader.GetString(0);
                                ClassStack.Children.Add(ProfName);
                            }
                            reader.Close(); // allows reader to be used again instead of creating reader2, 3, etc
                        }

                        query = "SELECT StartProfOptName, Choice FROM dbo.StartingProficienciesOptions" +
                                " WHERE ClassID = @ClassID3;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@ClassID3", selectedClass);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Label StartProf = new Label();
                            StartProf.TextColor = (Color)fontColor;
                            StartProf.Text = "Choose Optional Starting Proficiencies: ";
                            ClassStack.Children.Add(StartProf);
                            int choice = 0;
                            while (reader.Read())
                            {
                                Label ProfName = new Label();
                                ProfName.TextColor = (Color)fontColor;
                                ProfName.Text = reader.GetString(0);
                                ClassStack.Children.Add(ProfName);
                                choice = reader.GetInt32(1);
                            }
                            Label Choice = new Label();
                            Choice.TextColor = (Color)fontColor;
                            Choice.Text = "Choose " + choice;
                            ClassStack.Children.Add(Choice);
                            reader.Close(); // allows reader to be used again instead of creating reader2, 3, etc
                        }
                    }

                    // Creates the submit button
                    Button submit = new Button()
                    {
                        BackgroundColor = (Color)frameColor,
                        TextColor = (Color)fontColor,
                        Text = "Submit"
                    };
                    submit.Clicked += Submit;
                    ClassStack.Children.Add(submit);
                    mainPanel.Children.Add(ClassStack);
                }
            }
        }
        catch (Exception eSql)
        {
            DisplayAlert("Error!", eSql.Message, "OK"); // Should be removed and replaced before final product
            Debug.WriteLine("Exception: " + eSql.Message);
        }
    }

    /*
     * Function: Submit button function
     * Author: Anthony Rielly
     * Purpose: Sends the selected class back to the character sheet creation.
     * last Modified : 02/04/2023 9:00pm
     */
    private void Submit(object sender, EventArgs e)
    {
        characterSheet.characterclass = ClassName;
        Navigation.PushAsync(new CSheet(characterSheet));
    }

}