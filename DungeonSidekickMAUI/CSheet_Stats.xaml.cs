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
            Navigation.PushAsync(new RollForStatsPage(CharacterSheetcurrent));
        }
        public CSheet_Stats ()
		{
			InitializeComponent ();
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
            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6";

            string query = "INSERT INTO dbo.CharacterSheet" +
                "(CharacterName,Race,Class,Background,Alignment,PersonalityTraits,Ideals,Bonds,Flaws," +
                "FeaturesTraits,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma) VALUES" +
                "(@CharacterName,@Race,@Class,@Background,@Alignment,@PersonalityTraits,@Ideals,@Bonds," +
                "@Flaws,@FeaturesTraits,@Strength,@Dexterity,@Constitution,@Intelligence,@Wisdom,@Charisma);";
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
                            cmd.Parameters.AddWithValue("@Strength", CharacterSheetcurrent.strength);
                            cmd.Parameters.AddWithValue("@Dexterity", CharacterSheetcurrent.dexterity);
                            cmd.Parameters.AddWithValue("@Constitution", CharacterSheetcurrent.constitution);
                            cmd.Parameters.AddWithValue("@Intelligence", CharacterSheetcurrent.intelligence);
                            cmd.Parameters.AddWithValue("@Wisdom", CharacterSheetcurrent.wisdom);
                            cmd.Parameters.AddWithValue("@Charisma", CharacterSheetcurrent.charisma);
                            cmd.ExecuteNonQuery();
                            LoadCharacterSheetClass();
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            Navigation.PushAsync(new LandingPage());
        }
    }
}