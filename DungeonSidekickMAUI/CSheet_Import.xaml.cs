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
                    01/17/23 - Added edit button
                    01/21/23 - Updated variable names
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
                                Label CNameVal = new Label();
                                CNameVal.TextColor = Color.FromRgb(255, 255, 255);
                                CNameVal.Text = reader.GetString(0);

                                CharacterStack.Children.Add(CName);
                                CharacterStack.Children.Add(CNameVal);

                                Label Strength = new Label();
                                Strength.TextColor = Color.FromRgb(255, 255, 255);
                                Strength.Text = "Strength";
                                Label StrengthVal = new Label();
                                StrengthVal.TextColor = Color.FromRgb(255, 255, 255);
                                StrengthVal.Text = reader.GetInt32(1).ToString();
                                CharacterStack.Children.Add(Strength);
                                CharacterStack.Children.Add(StrengthVal);

                                Label Dexterity = new Label();
                                Dexterity.TextColor = Color.FromRgb(255, 255, 255);
                                Dexterity.Text = "Dexterity";
                                Label DexterityVal = new Label();
                                DexterityVal.TextColor = Color.FromRgb(255, 255, 255);
                                DexterityVal.Text = reader.GetInt32(2).ToString();
                                CharacterStack.Children.Add(Dexterity);
                                CharacterStack.Children.Add(DexterityVal);

                                Label Constitution = new Label();
                                Constitution.TextColor = Color.FromRgb(255, 255, 255);
                                Constitution.Text = "Constitution";
                                Label ConstitutionVal = new Label();
                                ConstitutionVal.TextColor = Color.FromRgb(255, 255, 255);
                                ConstitutionVal.Text = reader.GetInt32(3).ToString();

                                Label Intelligence = new Label();
                                Intelligence.TextColor = Color.FromRgb(255, 255, 255);
                                Intelligence.Text = "Intelligence";
                                Label IntelligenceVal = new Label();
                                IntelligenceVal.TextColor = Color.FromRgb(255, 255, 255);
                                IntelligenceVal.Text = reader.GetInt32(4).ToString();
                                CharacterStack.Children.Add(Constitution);
                                CharacterStack.Children.Add(ConstitutionVal);

                                Label Wisdom = new Label();
                                Wisdom.TextColor = Color.FromRgb(255, 255, 255);
                                Wisdom.Text = "Wisdom";
                                Label WisdomVal = new Label();
                                WisdomVal.TextColor = Color.FromRgb(255, 255, 255);
                                WisdomVal.Text = reader.GetInt32(5).ToString();
                                CharacterStack.Children.Add(Wisdom);
                                CharacterStack.Children.Add(WisdomVal);

                                Label Charisma = new Label();
                                Charisma.TextColor = Color.FromRgb(255, 255, 255);
                                Charisma.Text = "Charisma";
                                Label CharismaVal = new Label();
                                CharismaVal.TextColor = Color.FromRgb(255, 255, 255);
                                CharismaVal.Text = reader.GetInt32(6).ToString();
                                CharacterStack.Children.Add(Charisma);
                                CharacterStack.Children.Add(CharismaVal);

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
                                            sheet.charactername = CNameVal.Text;
                                            sheet.strength = StrengthVal.Text;
                                            sheet.dexterity = DexterityVal.Text;
                                            sheet.constitution = ConstitutionVal.Text;
                                            sheet.intelligence = IntelligenceVal.Text;
                                            sheet.wisdom = WisdomVal.Text;
                                            sheet.charisma = CharismaVal.Text;
                                            sheet.exists = true;
                                            Navigation.PushAsync(new CSheet(sheet));
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
