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
        private CharacterSheet CharacterSheetcurrent = CharacterSheet.Instance;
        public CSheet_Inventory()
        {
            InitializeComponent();
            Inventory.Text = "Placeholder";//CharacterSheetcurrent.characterclass;
        }
        private void SubmitStats(object sender, EventArgs e)
        {
            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6";

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
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@CharacterName", CharacterSheetcurrent.charactername);

                            //removing this for now because the previous thing submits everything (I dont think this page is even needed)
                            //cmd.ExecuteNonQuery();
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
            Navigation.PushAsync(new LandingPage(CharacterSheetcurrent));
        }
    }
}