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
        private TaskCompletionSource<bool> HealthPopupTask;

        public int chosenHealth { get; private set; }
        public int startingHealth { get; private set; }

        private bool m_NewAcc;

        private ImportedCharacterSheet CharacterSheetcurrent = ImportedCharacterSheet.Instance;
        /*
         * Function: RollForStats
         * Author: Kenny Rapp
         * Purpose: Navigate to the RollForStats
         * last Modified : 11/19/2023 3:25pm
         */
        private void RollForStats(object sender, EventArgs e)
        {
            LoadCharacterSheetClass();
            Navigation.PushAsync(new RollForStatsPage(m_NewAcc));
        }
        public CSheet_Stats (bool newAcc = false)
		{
			InitializeComponent ();

            // nav bar setup
            m_NewAcc = newAcc;
            if (!m_NewAcc)
            {
                Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
                NavigationCommands cmd = new NavigationCommands();
                NavigationPage.SetHasNavigationBar(this, true);
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
                NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());
            }
            LoadCharacterSheetPage();
            BindingContext = this;
        }


        /*
         * Function: LoadCharacterSheetPage
         * Author: Brendon Williams
         * Purpose: Loads the current character sheet to the text boxes on the screen
         * last Modified : 2/22/2024 8:49 pm
         */
        private void LoadCharacterSheetPage()
        {
            Strength.Text = CharacterSheetcurrent.c_Strength.ToString();
            Dexterity.Text = CharacterSheetcurrent.c_Dexterity.ToString();
            Constitution.Text = CharacterSheetcurrent.c_Constitution.ToString();
            Intelligence.Text = CharacterSheetcurrent.c_Intelligence.ToString();
            Wisdom.Text = CharacterSheetcurrent.c_Wisdom.ToString();
            Constitution.Text = CharacterSheetcurrent.c_Constitution.ToString();
            Charisma.Text = CharacterSheetcurrent.c_Charisma.ToString();
        }

        /*
         * Function: LoadCharacterSheetClass
         * Author: Brendon Williams
         * Purpose: Takes the text currently on the screen and puts it in the character sheet class
         * last Modified : 2/22/2024 8:49 pm
         */
        private void LoadCharacterSheetClass()
        {
            CharacterSheetcurrent.c_Strength = int.Parse(Strength.Text);
            CharacterSheetcurrent.c_Dexterity = int.Parse(Dexterity.Text);
            CharacterSheetcurrent.c_Constitution = int.Parse(Constitution.Text);
            CharacterSheetcurrent.c_Intelligence = int.Parse(Intelligence.Text);
            CharacterSheetcurrent.c_Wisdom = int.Parse(Wisdom.Text);
            CharacterSheetcurrent.c_Constitution = int.Parse(Constitution.Text);
            CharacterSheetcurrent.c_Charisma = int.Parse(Charisma.Text);
        }

        /*
         * Function: SubmitStats
         * Author: Brendon Williams
         * Purpose: Updates the character sheet class, then moves to next page
         * last Modified : 2/22/2024 8:49 pm
         */
        private async void SubmitStats(object sender, EventArgs e)
        {
            int chosenHP = 0;
            LoadCharacterSheetClass();
            Connection connection = Connection.connectionSingleton;

            string query = "INSERT INTO dbo.CharacterSheet" +
                "(UID,CharacterName,RaceId,ClassId,Background,Alignment,PersonalityTraits,Ideals,Bonds,Flaws," +
                "FeaturesTraits,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma,CurrentHP,TempHP) VALUES" +
                "(@UID,@CharacterName,@Race,@Class,@Background,@Alignment,@PersonalityTraits,@Ideals,@Bonds," +
                "@Flaws,@FeaturesTraits,@Strength,@Dexterity,@Constitution,@Intelligence,@Wisdom,@Charisma,@CurrentHP, @TempHP);" +
                "SELECT CAST(scope_identity() AS int)";
            bool successful = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connection.connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.Parameters.AddWithValue("@UID", Preferences.Default.Get("UserId", 0));
                            cmd.Parameters.AddWithValue("@CharacterName", CharacterSheetcurrent.c_Name);
                            cmd.Parameters.AddWithValue("@Race", CharacterSheetcurrent.c_Race);
                            cmd.Parameters.AddWithValue("@Class", CharacterSheetcurrent.c_Class);
                            cmd.Parameters.AddWithValue("@Background", CharacterSheetcurrent.c_Background);
                            cmd.Parameters.AddWithValue("@Alignment", CharacterSheetcurrent.c_Alignment);
                            cmd.Parameters.AddWithValue("@PersonalityTraits", CharacterSheetcurrent.c_PersonalityTraits);
                            cmd.Parameters.AddWithValue("@Ideals", CharacterSheetcurrent.c_Ideals);
                            cmd.Parameters.AddWithValue("@Bonds", CharacterSheetcurrent.c_Bonds);
                            cmd.Parameters.AddWithValue("@Flaws", CharacterSheetcurrent.c_Flaws);
                            cmd.Parameters.AddWithValue("@FeaturesTraits", CharacterSheetcurrent.c_FeaturesTraits);
                            int flag = 0;

                            if (int.Parse(Strength.Text) >= 0 && int.Parse(Strength.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Strength", CharacterSheetcurrent.c_Strength);
                            else
                                flag = 1;

                            if (int.Parse(Dexterity.Text) >= 0 && int.Parse(Dexterity.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Dexterity", CharacterSheetcurrent.c_Dexterity);
                            else
                                flag = 1;

                            if (int.Parse(Constitution.Text) >= 0 && int.Parse(Constitution.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Constitution", CharacterSheetcurrent.c_Constitution);
                            else
                                flag = 1;

                            if (int.Parse(Intelligence.Text) >= 0 && int.Parse(Intelligence.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Intelligence", CharacterSheetcurrent.c_Intelligence);
                            else
                                flag = 1;

                            if (int.Parse(Wisdom.Text) >= 0 && int.Parse(Wisdom.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Wisdom", CharacterSheetcurrent.c_Wisdom);
                            else
                                flag = 1;

                            if (int.Parse(Charisma.Text) >= 0 && int.Parse(Charisma.Text) <= 18)
                                cmd.Parameters.AddWithValue("@Charisma", CharacterSheetcurrent.c_Charisma);
                            else
                                flag = 1;


                            int HitDie = getHitDie(CharacterSheetcurrent.c_Class);
                            int stdHP = HitDie + ((CharacterSheetcurrent.c_Constitution - 10) % 2);

                            chosenHP = stdHP;

                            await ShowHealthPopupAndWaitAsync(stdHP);

                            cmd.Parameters.AddWithValue("@CurrentHP", chosenHP);
                            cmd.Parameters.AddWithValue("@TempHP", chosenHP);


                            cmd.CommandText = query;
                            if (flag != 1)
                            {
                                Preferences.Default.Set("CharacterID", (int)cmd.ExecuteScalar());
                                ImportedCharacterSheet.Save(CharacterSheetcurrent);
                                Navigation.PushAsync(new LandingPage());
                            }
                            else
                            {
                                DisplayAlert("Your stats are invalid.", "Please make sure they are between 0 and 18.", "Ok");
                                successful = false;
                            }
                        }
                    }
                    conn.Close();  
                }
            }
            catch (Exception eSql)
            {
                await DisplayAlert("Error!", eSql.Message, "OK");
                Debug.WriteLine("Exception: " + eSql.Message);
                successful = false;
            }
            if (successful)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connection.connectionString))
                    {
                        conn.Open();
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                query = "SELECT CharacterID FROM dbo.CharacterSheet " +
                                "WHERE UID = @UID " +
                                "AND CharacterName = @CharacterName " +
                                "AND RaceId = @Race " +
                                "AND ClassId = @Class " +
                                "AND Background = @Background " +
                                "AND Alignment = @Alignment " +
                                "AND PersonalityTraits = @PersonalityTraits " +
                                "AND Ideals = @Ideals " +
                                "AND Bonds = @Bonds " +
                                "AND Flaws = @Flaws " +
                                "AND FeaturesTraits = @FeaturesTraits " +
                                "AND Strength = @Strength " +
                                "AND Dexterity = @Dexterity " +
                                "AND Constitution = @Constitution " +
                                "AND Intelligence = @Intelligence " +
                                "AND Wisdom = @Wisdom " +
                                "AND Charisma = @Charisma " +
                                "AND CurrentHP = @CurrentHP " +
                                "AND TempHP = @TempHP ";

                                cmd.Parameters.AddWithValue("@UID", Preferences.Default.Get("UserId", 0));
                                cmd.Parameters.AddWithValue("@CharacterName", CharacterSheetcurrent.c_Name);
                                cmd.Parameters.AddWithValue("@Race", CharacterSheetcurrent.c_Race);
                                cmd.Parameters.AddWithValue("@Class", CharacterSheetcurrent.c_Class);
                                cmd.Parameters.AddWithValue("@Background", CharacterSheetcurrent.c_Background);
                                cmd.Parameters.AddWithValue("@Alignment", CharacterSheetcurrent.c_Alignment);
                                cmd.Parameters.AddWithValue("@PersonalityTraits", CharacterSheetcurrent.c_PersonalityTraits);
                                cmd.Parameters.AddWithValue("@Ideals", CharacterSheetcurrent.c_Ideals);
                                cmd.Parameters.AddWithValue("@Bonds", CharacterSheetcurrent.c_Bonds);
                                cmd.Parameters.AddWithValue("@Flaws", CharacterSheetcurrent.c_Flaws);
                                cmd.Parameters.AddWithValue("@FeaturesTraits", CharacterSheetcurrent.c_FeaturesTraits);

                                cmd.Parameters.AddWithValue("@Strength", CharacterSheetcurrent.c_Strength);

                                cmd.Parameters.AddWithValue("@Dexterity", CharacterSheetcurrent.c_Dexterity);

                                cmd.Parameters.AddWithValue("@Constitution", CharacterSheetcurrent.c_Constitution);
                                cmd.Parameters.AddWithValue("@Intelligence", CharacterSheetcurrent.c_Intelligence);
                                cmd.Parameters.AddWithValue("@Wisdom", CharacterSheetcurrent.c_Wisdom);
                                cmd.Parameters.AddWithValue("@Charisma", CharacterSheetcurrent.c_Charisma);
                                cmd.Parameters.AddWithValue("@CurrentHP", chosenHP);
                                cmd.Parameters.AddWithValue("@TempHP", chosenHP);
                                cmd.CommandText = query;

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {

                                    while (reader.Read())
                                    {
                                        Preferences.Default.Set("CharacterID", reader.GetInt32(0));
                                    }
                                }
                            }
                        }
                        conn.Close();

                    }
                }
                catch (Exception eSql)
                {
                    await DisplayAlert("Error!", eSql.Message, "OK");
                    Debug.WriteLine("Exception: " + eSql.Message);

                }
            }


        }
        private int getHitDie(int classID)
        {
            Connection connection = Connection.connectionSingleton;
            string query = "SELECT HitDie FROM dbo.ClassLookup WHERE ClassID = @ClassID";
            int HitDie = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connection.connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@ClassID", classID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    HitDie = reader.GetInt32(0);
                                }
                            }
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
            return HitDie;
        }

        public async Task ShowHealthPopupAndWaitAsync(int startingHealth)
        {
            StartingHealthLabel.Text = $"Normally your health would have been {startingHealth}";
            HealthPopupTask = new TaskCompletionSource<bool>();
            HealthPopup.IsVisible = true;
            await HealthPopupTask.Task;
            HealthPopup.IsVisible = false;
        }

        private void StartingHealthChosen(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            list.Add(chosenHealthEntry.Text);
            if (!GlobalFunctions.entryCheck(list, 1))
                return;
            chosenHealth = int.TryParse(chosenHealthEntry.Text, out int result) ? result : 0;
            HealthPopupTask.SetResult(true);
        }
    }
}