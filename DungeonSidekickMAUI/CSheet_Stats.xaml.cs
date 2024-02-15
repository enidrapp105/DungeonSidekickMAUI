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
        private CharacterSheet CharacterSheetcurrent;
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
        public CSheet_Stats (CharacterSheet characterSheet)
		{
			InitializeComponent ();
            CharacterSheetcurrent = characterSheet;
            LoadCharacterSheetPage(CharacterSheetcurrent);
        }
        

        //Loads the character sheet to the user's viewable page
        private void LoadCharacterSheetPage(CharacterSheet characterSheet)
        {
            Strength.Text = characterSheet.strength;
            Dexterity.Text = characterSheet.dexterity;
            Constitution.Text = characterSheet.constitution;
            Intelligence.Text = characterSheet.intelligence;
            Wisdom.Text = characterSheet.wisdom;
            Constitution.Text = characterSheet.constitution;
            Charisma.Text = characterSheet.charisma;
        }
        private void LoadCharacterSheetClass()
        {
            CharacterSheetcurrent.strength = Strength.Text;
            CharacterSheetcurrent.dexterity = Dexterity.Text;
            CharacterSheetcurrent.constitution = Constitution.Text;
            CharacterSheetcurrent.intelligence = Intelligence.Text;
            CharacterSheetcurrent.wisdom = Wisdom.Text;
            CharacterSheetcurrent.constitution = Constitution.Text;
            CharacterSheetcurrent.charisma = Charisma.Text;
        }

        private void SubmitStats(object sender, EventArgs e)
        {

            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

            string query = "INSERT INTO dbo.CharacterSheet" +
                "(CharacterName,PlayerName,Race,Class,Background,Alignment,PersonalityTraits,Ideals,Bonds,Flaws," +
                "FeaturesTraits,Equipment,Proficiencies,Attacks,Spells,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma) VALUES" +
                "(@CharacterName,@PlayerName,@Race,@Class,@Background,@Alignment,@PersonalityTraits,@Ideals,@Bonds," +
                "@Flaws,@FeaturesTraits,@Equipment,@Proficiencies,@Attacks,@Spells,@Strength,@Dexterity,@Constitution,@Intelligence,@Wisdom,@Charisma);";

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
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            Navigation.PushAsync(new CSheet_Inventory(CharacterSheetcurrent));
        }
    }
}