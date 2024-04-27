using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DungeonSidekickMAUI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CSheet_Stats : ContentPage
	{
        private CharacterSheet CharacterSheetcurrent = CharacterSheet.Instance;
        /*
         * Function: RollForStats
         * Author: Kenny Rapp
         * Purpose: Navigate to the RollForStats
         * last Modified : 11/19/2023 3:25pm
         */
        private void RollForStats(object sender, EventArgs e)
        {
            LoadCharacterSheetClass();
            Navigation.PushAsync(new RollForStatsPage());
        }
        public CSheet_Stats ()
		{
			InitializeComponent ();

            // nav bar setup
            Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
            NavigationCommands cmd = new NavigationCommands();
            NavigationPage.SetHasNavigationBar(this, true);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
            NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());

            LoadCharacterSheetPage();
        }


        /*
         * Function: LoadCharacterSheetPage
         * Author: Brendon Williams
         * Purpose: Loads the current character sheet to the text boxes on the screen
         * last Modified : 2/22/2024 8:49 pm
         */
        private void LoadCharacterSheetPage()
        {
            Strength.Text = CharacterSheetcurrent.strength.ToString();
            Dexterity.Text = CharacterSheetcurrent.dexterity.ToString();
            Constitution.Text = CharacterSheetcurrent.constitution.ToString();
            Intelligence.Text = CharacterSheetcurrent.intelligence.ToString();
            Wisdom.Text = CharacterSheetcurrent.wisdom.ToString();
            Constitution.Text = CharacterSheetcurrent.constitution.ToString();
            Charisma.Text = CharacterSheetcurrent.charisma.ToString();
        }

        /*
         * Function: LoadCharacterSheetClass
         * Author: Brendon Williams
         * Purpose: Takes the text currently on the screen and puts it in the character sheet class
         * last Modified : 2/22/2024 8:49 pm
         */
        private void LoadCharacterSheetClass()
        {
            CharacterSheetcurrent.strength = int.Parse(Strength.Text);
            CharacterSheetcurrent.dexterity = int.Parse(Dexterity.Text);
            CharacterSheetcurrent.constitution = int.Parse(Constitution.Text);
            CharacterSheetcurrent.intelligence = int.Parse(Intelligence.Text);
            CharacterSheetcurrent.wisdom = int.Parse(Wisdom.Text);
            CharacterSheetcurrent.constitution = int.Parse(Constitution.Text);
            CharacterSheetcurrent.charisma = int.Parse(Charisma.Text);
        }

        /*
         * Function: SubmitStats
         * Author: Brendon Williams
         * Purpose: Updates the character sheet class, then moves to next page
         * last Modified : 2/22/2024 8:49 pm
         */
        private void SubmitStats(object sender, EventArgs e)
        {
            LoadCharacterSheetClass();
            Connection connection = Connection.connectionSingleton;

            string query = "INSERT INTO dbo.CharacterSheet" +
                "(UID,CharacterName,RaceId,ClassId,Background,Alignment,PersonalityTraits,Ideals,Bonds,Flaws," +
                "FeaturesTraits,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma) VALUES" +
                "(@UID,@CharacterName,@Race,@Class,@Background,@Alignment,@PersonalityTraits,@Ideals,@Bonds," +
                "@Flaws,@FeaturesTraits,@Strength,@Dexterity,@Constitution,@Intelligence,@Wisdom,@Charisma);" +
                "SELECT CAST(scope_identity() AS int)";

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
                            cmd.Parameters.AddWithValue("@UID", Preferences.Default.Get("UserId", 0));
                            cmd.Parameters.AddWithValue("@CharacterName", CharacterSheetcurrent.charactername);
                            cmd.Parameters.AddWithValue("@Race", CharacterSheetcurrent.race);
                            cmd.Parameters.AddWithValue("@Class", CharacterSheetcurrent.characterclass);
                            cmd.Parameters.AddWithValue("@Background", CharacterSheetcurrent.background);
                            cmd.Parameters.AddWithValue("@Alignment", CharacterSheetcurrent.alignment);
                            cmd.Parameters.AddWithValue("@PersonalityTraits", CharacterSheetcurrent.personalitytraits);
                            cmd.Parameters.AddWithValue("@Ideals", CharacterSheetcurrent.ideals);
                            cmd.Parameters.AddWithValue("@Bonds", CharacterSheetcurrent.bonds);
                            cmd.Parameters.AddWithValue("@Flaws", CharacterSheetcurrent.flaws);
                            cmd.Parameters.AddWithValue("@FeaturesTraits", CharacterSheetcurrent.featurestraits);
                            int flag = 0;

                            if (int.Parse(Strength.Text) >= 0 && int.Parse(Strength.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Strength", CharacterSheetcurrent.strength);
                            else
                                flag = 1;

                            if (int.Parse(Dexterity.Text) >= 0 && int.Parse(Dexterity.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Dexterity", CharacterSheetcurrent.dexterity);
                            else
                                flag = 1;

                            if (int.Parse(Constitution.Text) >= 0 && int.Parse(Constitution.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Constitution", CharacterSheetcurrent.constitution);
                            else
                                flag = 1;

                            if (int.Parse(Intelligence.Text) >= 0 && int.Parse(Intelligence.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Intelligence", CharacterSheetcurrent.intelligence);
                            else
                                flag = 1;

                            if (int.Parse(Wisdom.Text) >= 0 && int.Parse(Wisdom.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Wisdom", CharacterSheetcurrent.wisdom);
                            else
                                flag = 1;

                            if (int.Parse(Charisma.Text) >= 0 && int.Parse(Charisma.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Charisma", CharacterSheetcurrent.charisma);
                            else
                                flag = 1;
                            if (flag != 1)
                            {
                                Preferences.Default.Set("CharacterID", (int)cmd.ExecuteScalar());
                            }
                            else
                                DisplayAlert("Your stats are invalid.", "Please make sure they are between 0 and 18.", "Ok");
                            
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception eSql)
            {
                DisplayAlert("Error!", eSql.Message, "OK");
                Debug.WriteLine("Exception: " + eSql.Message);
            }

            Navigation.PushAsync(new LandingPage());

        }
    }
}