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

        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False; MultipleActiveResultSets=true;";
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
                    Color PrimaryColor = (Color)Application.Current.Resources["PrimaryColor"];
                    Color SecondaryColor = (Color)Application.Current.Resources["SecondaryColor"];
                    Color TrinaryColor = (Color)Application.Current.Resources["TrinaryColor"];
                    Color fontColor = (Color)Application.Current.Resources["FontC"];
                    //var hasValue = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("FontC", out object fontColor);
                    //var hasValue2 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("SecondaryColor", out object SecondaryColor);
                    //var hasValue3 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("TrinaryColor", out object TrinaryColor);
                    //var hasValue4 = Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("PrimaryColor", out object PrimaryColor);
                    ClassStack.BackgroundColor = PrimaryColor;
                    Frame optionalSkillsFrame = new Frame();
                    Frame savingThrowsFrame = new Frame();
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
                                Class.TextColor = fontColor;
                                ClassName = reader.GetString(0);
                                Class.Text = ClassName;

                                Frame frame = new Frame()
                                {
                                    BackgroundColor = SecondaryColor,
                                    Padding = 24,
                                    CornerRadius = 0,
                                    Content = Class
                                };
                                Label HitDie = new Label();
                                HitDie.HorizontalTextAlignment = TextAlignment.Center;
                                HitDie.TextColor = fontColor;
                                HitDie.Text = "Hit Die: " + reader.GetInt32(1);
                                ClassStack.Children.Add(Class);
                                ClassStack.Children.Add(HitDie);
                            }
                            reader.Close(); // allows reader to be used again instead of creating innerReader, 3, etc
                        }

                        query = "SELECT ProfID, Optional, Choice FROM dbo.ClassProficienciesLookup" +
                                " WHERE ClassID = @ClassID2;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@ClassID2", selectedClass);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            StackLayout savingThrows = new StackLayout();
                            savingThrows.Children.Add(new Label { Text="Saving Throws:", TextColor = fontColor });
                            StackLayout optionalSkills = new StackLayout();
                            savingThrowsFrame.BackgroundColor = SecondaryColor;
                            optionalSkillsFrame.BackgroundColor = SecondaryColor;
                            
                            int newOption = 0;
                            while (reader.Read())
                            {
                                int Id =  reader.GetInt32(0);
                                int optional = reader.GetInt32(1);
                                int choice = reader.GetInt32(2);
                                if (optional != newOption && optional != 0)
                                {
                                    if(optional == 1)
                                    {
                                        Label StartProf = new Label();
                                        StartProf.TextColor = fontColor;
                                        //StartProf.BackgroundColor = PrimaryColor;
                                        StartProf.Text = "Choose Optional Starting Skills: ";
                                        //ClassStack.Children.Add(StartProf);
                                        optionalSkills.Add(StartProf);
                                    }
                                    Label Choice = new Label();
                                    Choice.TextColor = fontColor;
                                    Choice.Text = "Choose " + choice + " (for standard games)";
                                    //ClassStack.Children.Add(Choice);
                                    optionalSkills.Add(Choice);
                                }
                                newOption = optional;

                                string innerQuery = "SELECT ProfName FROM dbo.ProficienciesLookup" +
                                " WHERE ProfID = @ProfID;";
                                try
                                {
                                    
                                    using (SqlConnection innerConn = new SqlConnection(connectionString))
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
                                                        ProfName.TextColor = fontColor;
                                                        ProfName.Text = innerReader.GetString(0);
                                                        if ((innerReader.GetString(0)).Contains("Skill"))
                                                        {
                                                            optionalSkills.Children.Add(ProfName);
                                                        }
                                                        else if ((innerReader.GetString(0)).Contains("Saving Throw"))
                                                        {
                                                            savingThrows.Children.Add(ProfName);
                                                        }
                                                        else
                                                        {
                                                            ClassStack.Children.Add(ProfName);
                                                        }
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
                            savingThrowsFrame.Content = savingThrows;
                            optionalSkillsFrame.Content = optionalSkills;
                        }
                    }

                    // Creates the submit button
                    Button submit = new Button()
                    {
                        BackgroundColor = SecondaryColor,
                        TextColor = fontColor,
                        Text = "Submit"
                    };
                    submit.Clicked += Submit;
                    ClassStack.Children.Add(savingThrowsFrame);
                    ClassStack.Children.Add(optionalSkillsFrame);
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