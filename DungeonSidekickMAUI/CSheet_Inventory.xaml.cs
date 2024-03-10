using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DungeonSidekickMAUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CSheet_Inventory : ContentPage
    {
        private CharacterSheet CharacterSheetcurrent;
        public CSheet_Inventory(CharacterSheet sheet)
        {
            CharacterSheetcurrent = sheet;
            InitializeComponent();
            Inventory.Text = CharacterSheetcurrent.characterclass.ToString();
        }
        private void SubmitStats(object sender, EventArgs e)
        {
            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6";

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
                            cmd.Parameters.AddWithValue("@Equipment", Inventory.Text);
                            cmd.Parameters.AddWithValue("@Proficiencies", Proficiencies.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            Navigation.PushAsync(new LandingPage(CharacterSheetcurrent));
        }
    }
}