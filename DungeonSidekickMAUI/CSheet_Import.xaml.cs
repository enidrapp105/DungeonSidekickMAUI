using Microsoft.Maui.Controls.PlatformConfiguration;

namespace DungeonSidekickMAUI;
/*
    Author: Jonathan Raffaelly
    Date created: 11/19/23
    Function name: Character Sheet Import
    Purpose: Used to import saved character sheet data
    Modifications:  11/19/23 - Created sheet and added functionality.
                    11/20/23 - Got import function to work properly.
                    12/02/23 - Changed background to black and text to white
 */

using System.Diagnostics;
using Microsoft.Data.SqlClient;
public partial class CSheet_Import : ContentPage
{
    public CSheet_Import()
    {
        InitializeComponent();

    }
    private void ImportSheet(object sender, EventArgs e)
    {
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

        string query = "SELECT CharacterName,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma FROM dbo.CharacterSheet" +
            " WHERE PlayerName = @PlayerName;";
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
                        cmd.Parameters.AddWithValue("@PlayerName", PName.Text);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StackLayout CharacterStack = new StackLayout();

                                Label CName = new Label();
                                CName.TextColor = Color.FromRgb(255, 255, 255);
                                CName.Text = "Character Name";
                                Label CName2 = new Label();
                                CName2.TextColor = Color.FromRgb(255, 255, 255);
                                CName2.Text = reader.GetString(0);

                                CharacterStack.Children.Add(CName);
                                CharacterStack.Children.Add(CName2);

                                Label Strength = new Label();
                                Strength.TextColor = Color.FromRgb(255, 255, 255);
                                Strength.Text = "Strength";
                                Label Strength2 = new Label();
                                Strength2.TextColor = Color.FromRgb(255, 255, 255);
                                Strength2.Text = reader.GetInt32(1).ToString();
                                CharacterStack.Children.Add(Strength);
                                CharacterStack.Children.Add(Strength2);

                                Label Dexterity = new Label();
                                Dexterity.TextColor = Color.FromRgb(255, 255, 255);
                                Dexterity.Text = "Dexterity";
                                Label Dexterity2 = new Label();
                                Dexterity2.TextColor = Color.FromRgb(255, 255, 255);
                                Dexterity2.Text = reader.GetInt32(2).ToString();
                                CharacterStack.Children.Add(Dexterity);
                                CharacterStack.Children.Add(Dexterity2);

                                Label Constitution = new Label();
                                Constitution.TextColor = Color.FromRgb(255, 255, 255);
                                Constitution.Text = "Constitution";
                                Label Constitution2 = new Label();
                                Constitution2.TextColor = Color.FromRgb(255, 255, 255);
                                Constitution2.Text = reader.GetInt32(3).ToString();

                                Label Intelligence = new Label();
                                Intelligence.TextColor = Color.FromRgb(255, 255, 255);
                                Intelligence.Text = "Intelligence";
                                Label Intelligence2 = new Label();
                                Intelligence2.TextColor = Color.FromRgb(255, 255, 255);
                                Intelligence2.Text = reader.GetInt32(4).ToString();
                                CharacterStack.Children.Add(Constitution);
                                CharacterStack.Children.Add(Constitution2);

                                Label Wisdom = new Label();
                                Wisdom.TextColor = Color.FromRgb(255, 255, 255);
                                Wisdom.Text = "Wisdom";
                                Label Wisdom2 = new Label();
                                Wisdom2.TextColor = Color.FromRgb(255, 255, 255);
                                Wisdom2.Text = reader.GetInt32(5).ToString();
                                CharacterStack.Children.Add(Wisdom);
                                CharacterStack.Children.Add(Wisdom2);

                                Label Charisma = new Label();
                                Charisma.TextColor = Color.FromRgb(255, 255, 255);
                                Charisma.Text = "Charisma";
                                Label Charisma2 = new Label();
                                Charisma2.TextColor = Color.FromRgb(255, 255, 255);
                                Charisma2.Text = reader.GetInt32(6).ToString();
                                CharacterStack.Children.Add(Charisma);
                                CharacterStack.Children.Add(Charisma2);

                                Button Edit = new Button {
                                    TextColor = Color.FromRgb(0, 0, 0),
                                    Text = "Edit",
                                    BackgroundColor = Color.FromRgb(255, 255, 255),
                                    Command = new Command
                                    (
                                        execute: async() =>
                                        {
                                            CharacterSheet sheet = new CharacterSheet();
                                            sheet.playername = PName.Text;
                                            sheet.charactername = CName2.Text;
                                            sheet.strength = Strength2.Text;
                                            sheet.dexterity = Dexterity2.Text;
                                            sheet.constitution = Constitution2.Text;
                                            sheet.intelligence = Intelligence2.Text;
                                            sheet.wisdom = Wisdom2.Text;
                                            sheet.charisma = Charisma2.Text;
                                            Navigation.PushAsync(new CSheet(sheet, true));
                                        }
                                    )
                                };
                                CharacterStack.Children.Add(Edit);
                                BoxView line = new BoxView
                                {
                                    Color = Color.FromRgb(255,255,255),
                                    HeightRequest = 1,
                                    HorizontalOptions = LayoutOptions.FillAndExpand
                                };
                                CharacterStack.Children.Add(line);

                                MainPanel.Children.Add(CharacterStack);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception eSql)
        {
            Debug.WriteLine("Exception: " + eSql.Message);
        }
    }
}
