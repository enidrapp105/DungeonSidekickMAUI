using LocalAuthentication;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//may be replacing this one, as it is depreciated
namespace DungeonSidekickMAUI
{
    public class CharacterSheet
    {
        // Private static instance variable
        private static CharacterSheet instance;

        // Private constructor to prevent instantiation
        private CharacterSheet() 
        {
            this.Purge();
        }

        // Public static method to access the singleton instance
        public static CharacterSheet Instance
        {
            get
            {
                // Lazy initialization: create the instance if it doesn't exist
                // Not thread safe, but I think that is a bit overkill
                if (instance == null)
                {
                    instance = new CharacterSheet();
                }
                return instance;
            }
        }

        // Properties
        public string? charactername { get; set; }
        public int race { get; set; }
        public int characterclass { get; set; }
        public string? raceName { get; set; }
        public string? className { get; set; }
        public string? background { get; set; }
        public string? alignment { get; set; }
        public string? personalitytraits { get; set; }
        public string? ideals { get; set; }
        public string? bonds { get; set; }
        public string? flaws { get; set; }
        public string? featurestraits { get; set; }
        public string? proficiencies { get; set; }
        public string? attacks { get; set; }
        public int strength { get; set; }
        public int dexterity { get; set; }
        public int constitution { get; set; }
        public int intelligence { get; set; }
        public int wisdom { get; set; }
        public int charisma { get; set; }
        public bool exists { get; set; }
        public Inventory inv { get; set; } // Maybe make this a singleton? No, that would break too many things.
        public bool WEquipped { get; set; } // Flag to check if a weapon is currently equipped.
        public int WEquippedID { get; set; } // Integer to keep track of currently equipped weapon.
        public string damageDice { get; set; }
        /*
         * Function: Purge
         * Author: Brendon Williams
         * Purpose: Resets the instance back to nulls. This way, we can reset the instance easily
         * last Modified : 2/22/2024 8:15 pm
         */
        public void Purge()
        {
            charactername = null;
            race = -1;
            characterclass = -1;
            background = null;
            alignment = null;
            personalitytraits = null;
            ideals = null;
            bonds = null;
            flaws = null;
            featurestraits = null;
            proficiencies = null;
            attacks = null;
            strength = 0;
            dexterity = 0;
            constitution = 0;
            intelligence = 0;
            wisdom = 0;
            charisma = 0;
            exists = false;
            damageDice = 0;
            inv = new Inventory();
        }
        /*
         * Function: Equip Item
         * Author: Thomas Hewitt
         * Purpose: Equips the specified item to the current character, further modifying their stats.
         * last Modified : 4/13/2024 8:52 am
         */
        public void EquipItem(int ID, int ETypeID)
        {
            if(ETypeID == 0) // Weapon Check
            {
                WEquipped = true;
                WEquippedID = ID;
                string query = "SELECT damageDice FROM dbo.Weapon" +
                " WHERE WeaponID = @Id;";
                Connection connection = Connection.connectionSingleton;
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

                                // grabs ID from weapon list
                                cmd.Parameters.AddWithValue("@Id", ID);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        damageDice = reader.GetString(0);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception eSql)
                {
                    //DisplayAlert("Error!", eSql.Message, "OK");
                    Debug.WriteLine("Exception: " + eSql.Message);
                }
            }


        }

    }

}
