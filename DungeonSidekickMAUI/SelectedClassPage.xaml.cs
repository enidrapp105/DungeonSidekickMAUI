using System.Diagnostics;
using Microsoft.Data.SqlClient;
//using Network;
namespace DungeonSidekickMAUI;

public partial class SelectedClassPage : ContentPage
{
    CharacterSheet characterSheet;
    DndClass m_class;
    public SelectedClassPage(CharacterSheet characterSheet, int selectedClass)
    {
        string ClassName;
        this.characterSheet = characterSheet;
        //this.SelectedClass = selectedclass;
        //Classlabel.Text = SelectedClass.ClassName;
        //ClassDescLabel.Text = SelectedClass.ClassDesc;
        //ClassHitDieLabel.Text = "Hit Die:" + SelectedClass.HitDie;
        //this.characterSheet = characterSheet;
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
                                int hit = 0;
                                HitDie.Text = "Hit Die: " + reader.GetInt32(1);
                                ClassStack.Children.Add(Class);
                                ClassStack.Children.Add(HitDie);
                            }

                        }

                        query = "SELECT StartProfName FROM dbo.StartingProficiencies" +
                                " WHERE ClassID = @ClassID4;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@ClassID4", selectedClass);
                        using (SqlDataReader reader4 = cmd.ExecuteReader())
                        {
                            Label StartProf = new Label();
                            StartProf.TextColor = (Color)fontColor;
                            StartProf.Text = "Starting Proficiencies: ";
                            ClassStack.Children.Add(StartProf);
                            while (reader4.Read())
                            {
                                Label ProfName = new Label();
                                ProfName.TextColor = (Color)fontColor;
                                ProfName.Text = reader4.GetString(0);
                                ClassStack.Children.Add(ProfName);
                            }
                        }

                        query = "SELECT StartProfOptName, Choice FROM dbo.StartingProficienciesOptions" +
                                " WHERE ClassID = @ClassID5;";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@ClassID5", selectedClass);
                        using (SqlDataReader reader5 = cmd.ExecuteReader())
                        {
                            Label StartProf = new Label();
                            StartProf.TextColor = (Color)fontColor;
                            StartProf.Text = "Choose Optional Starting Proficiencies: ";
                            ClassStack.Children.Add(StartProf);
                            int choice = 0;
                            while (reader5.Read())
                            {
                                Label ProfName = new Label();
                                ProfName.TextColor = (Color)fontColor;
                                ProfName.Text = reader5.GetString(0);
                                ClassStack.Children.Add(ProfName);
                                choice = reader5.GetInt32(1);
                            }
                            Label Choice = new Label();
                            Choice.TextColor = (Color)fontColor;
                            Choice.Text = "Choose " + choice;
                            ClassStack.Children.Add(Choice);
                        }
                    }
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
            DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
        }
    }
    private void Submit(object sender, EventArgs e)
    {
        characterSheet.characterclass = m_class;
        Navigation.PushAsync(new CSheet(characterSheet));
    }

}