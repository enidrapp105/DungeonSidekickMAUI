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
            LoadCharacterSheetPage(CharacterSheetcurrent);
        }
        

        //Loads the character sheet to the user's viewable page
        private void LoadCharacterSheetPage(CharacterSheet characterSheet)
        {
            Strength.Text = characterSheet.strength.ToString();
            Dexterity.Text = characterSheet.dexterity.ToString();
            Constitution.Text = characterSheet.constitution.ToString();
            Intelligence.Text = characterSheet.intelligence.ToString();
            Wisdom.Text = characterSheet.wisdom.ToString();
            Constitution.Text = characterSheet.constitution.ToString();
            Charisma.Text = characterSheet.charisma.ToString();
        }
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

        private void SubmitStats(object sender, EventArgs e)
        {

            LoadCharacterSheetClass();

            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

            string query = "INSERT INTO dbo.CharacterSheet" +
                "(CharacterName,RaceId,ClassId,Background,Alignment,PersonalityTraits,Ideals,Bonds,Flaws," +
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
                            cmd.CommandText = query;
                            int flag = 0;

                            if (int.Parse(Strength.Text) >= 0 && int.Parse(Strength.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Strength", int.Parse(Strength.Text));
                            else
                                flag = 1;

                            if (int.Parse(Dexterity.Text) >= 0 && int.Parse(Dexterity.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Dexterity", int.Parse(Dexterity.Text));
                            else
                                flag = 1;

                            if (int.Parse(Constitution.Text) >= 0 && int.Parse(Constitution.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Constitution", int.Parse(Constitution.Text));
                            else
                                flag = 1;

                            if (int.Parse(Intelligence.Text) >= 0 && int.Parse(Intelligence.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Intelligence", int.Parse(Intelligence.Text));
                            else
                                flag = 1;

                            if (int.Parse(Wisdom.Text) >= 0 && int.Parse(Wisdom.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Wisdom", int.Parse(Wisdom.Text));
                            else
                                flag = 1;

                            if (int.Parse(Charisma.Text) >= 0 && int.Parse(Charisma.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Charisma", int.Parse(Charisma.Text));
                            else
                                flag = 1;

                            if (flag == 0)
                            {
                                cmd.ExecuteNonQuery();
                            }
                            else
                                Console.WriteLine("One of your stats is either below 0 or above 20, please move it to between this range.");
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                DisplayAlert("Error!", eSql.Message, "OK");
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            Navigation.PushAsync(new CSheet_Inventory());
        }
    }
}