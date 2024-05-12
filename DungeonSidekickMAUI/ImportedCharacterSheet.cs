using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    public class ImportedCharacterSheet
    {
        private static ImportedCharacterSheet instance;

        // Private constructor to prevent instantiation
        private ImportedCharacterSheet()
        {
            this.Purge();
        }

        // Public static method to access the singleton instance
        public static ImportedCharacterSheet Instance
        {
            get
            {
                // Lazy initialization: create the instance if it doesn't exist
                // Not thread safe, but I think that is a bit overkill
                if (instance == null)
                {
                    instance = new ImportedCharacterSheet();
                }
                return instance;
            }
        }

        

        //constructor
        public ImportedCharacterSheet(int p_UID, int p_CharacterID)
        {
            this.p_UID = p_UID;
            this.p_CharacterID = p_CharacterID;
        }

        //public int GetUID()
        //{
        //    return p_UID;
        //}
        //public int GetCharacterID()
        //{
        //    return p_CharacterID;
        //}

        public string? c_Name { get; set; }

        public int c_Class { get; set; } //currently stores an id
        public int c_Race { get; set; } //currently stores an id

        public int c_Level { get; set; }

        public string? c_Background { get; set; } //may have other uses later on
        public string? c_Alignment { get; set; }
        public string? c_PersonalityTraits { get; set; }
        public string? c_Ideals { get; set; }
        public string? c_Bonds { get; set; }
        public string? c_Flaws { get; set; }
        public string? c_FeaturesTraits { get; set; }

        public int c_Equipment { get; set; } //currently stores an id


        //character stats
        public int c_Strength { get; set; }
        public int c_Dexterity { get; set; }
        public int c_Constitution { get; set; }
        public int c_Intelligence { get; set; }
        public int c_Wisdom { get; set; }
        public int c_Charisma { get; set; }

        //character combat stats
        public int c_CurrentHealth { get; set; }
        public int c_TemporaryHealth { get; set; }
        public int c_ArmorClass { get; set; }
        public int c_Initiative { get; set; }
        public int c_Speed { get; set; }
        public int c_HitDice { get; set; }

        //saves
        public int c_StrengthSave { get; set; }
        public int c_DexteritySave { get; set; }
        public int c_ConstitutionSave { get; set; }
        public int c_IntelligenceSave { get; set; }
        public int c_WisdomSave { get; set; }
        public int c_CharismaSave { get; set; }

        //skills
        public int c_Acrobatics { get; set; }
        public int c_AnimalHandling { get; set; }
        public int c_Arcana { get; set; }
        public int c_Athletics { get; set; }
        public int c_Deception { get; set; }
        public int c_History { get; set; }
        public int c_Insight { get; set; }
        public int c_Intimidation { get; set; }
        public int c_Investigation { get; set; }
        public int c_Medicine { get; set; }
        public int c_Nature { get; set; }
        public int c_Perception { get; set; }
        public int c_Performance { get; set; }
        public int c_Persuasion { get; set; }
        public int c_Religion { get; set; }
        public int c_SleightOfHand { get; set; }
        public int c_Stealth { get; set; }
        public int c_Survival { get; set; }

        //storing these in a list is going to be slightly better for visibility, without adding too much more complexity to accessing it.
        //public List<String>? c_Proficiencies;
        //public List<String>? c_Equipment; //this may need heavy changes, possibly set this as a key value pair list so that we could access data as needed, and not just the names

        public int c_PassiveWisdom { get; set; }


        //debateable for keeping
        public int p_UID { get; private set; }
        public int p_CharacterID { get; private set; }

        public bool exists { get; set; }

        public string c_RaceName { get; set; }
        public string c_ClassName { get; set; }

        public Inventory c_inv { get; set; } // I have ideas for this, but I didn't get around to implementing them this sprint: Thomas
        public bool c_WEquipped { get; set; } // Flag to check if a weapon is currently equipped.
        public int c_WEquippedID { get; set; } // Integer to keep track of currently equipped weapon.
        public bool c_SEquipped { get; set; } //track if spell is equipped
        public int c_SEquippedID { get; set; } //track id of equipped spell
        public string? c_damageDice { get; set; }
        public bool c_EEquipped { get; set; }
        public int c_EEquippedID { get; set; }
        public int c_ACBoost { get; set; } // The increased armor from equipped armor.


        //******************************************** FUNCTIONS *******************************************************
        public void Purge()
        {
            c_Name = null;
            c_Race = -1;
            c_Class = -1;
            c_Background = null;
            c_Alignment = null;
            c_PersonalityTraits = null;
            c_Ideals = null;
            c_Bonds = null;
            c_Flaws = null;
            c_FeaturesTraits = null;
            c_Strength = 0;
            c_Dexterity = 0;
            c_Constitution = 0;
            c_Intelligence = 0;
            c_Wisdom = 0;
            c_Charisma = 0;
            exists = false;
            c_damageDice = null;
            c_inv = new Inventory();
            c_WEquipped = false;
            c_WEquippedID = -1;
            c_SEquipped = false;
            c_SEquippedID = -1;
            c_damageDice = null;
            c_EEquipped = false;
            c_EEquippedID = -1;
            c_ACBoost = 0;
        }

        // for storing in preferences
        public static void Save(ImportedCharacterSheet character)
        {
            Preferences.Default.Set("UserCharacter", JsonSerializer.Serialize(character));
        }

        // for returing to preferences
        public static ImportedCharacterSheet Load()
        {
            return JsonSerializer.Deserialize<ImportedCharacterSheet>(Preferences.Default.Get("UserCharacter", ""));
        }

        public void Export()
        {
            Connection connection = Connection.connectionSingleton;

            string query = "UPDATE dbo.CharacterSheet" +
                " SET CharacterName = @CharacterName, RaceID = @RaceID, ClassID = @ClassID, Level = @Level, Background = @Background, Alignment = @Alignment, PersonalityTraits = @PersonalityTraits, Ideals = @Ideals, Bonds = @Bonds, Flaws = @Flaws," +
                " FeaturesTraits = @FeaturesTraits, Strength = @Strength, Dexterity = @Dexterity, Constitution = @Constitution, Intelligence = @Intelligence, Wisdom = @Wisdom, Charisma = @Charisma, CurrentHP = @CurrentHP, TempHP = @TempHP, AC = @AC, Initiative = @Initiative," +
                " Speed = @Speed, HitDice = @HitDice, StrSave = @StrSave, DexSave = @DexSave, ConSave = @ConSave, IntSave = @IntSave, WisSave = @WisSave, ChaSave = @ChaSave, Acrobatics = @Acrobatics, AnimalHandling = @AnimalHandling, Arcana = @Arcana, Athletics = @Athletics, Deception = @Deception," +
                " History = @History, Insight = @Insight, Intimidation = @Intimidation, Investigation = @Investigation, Medicine = @Medicine, Nature = @Nature, Perception = @Perception, Performance = @Performance, Persuasion = @Persuasion, Religion = @Religion, Sleight = @Sleight, Stealth = @Stealth," +
                " Survival = @Survival, PassiveWisdom = @PassiveWisdom" +
                " WHERE CharacterID = @CharacterID;";


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
                            cmd.Parameters.Add("@CharacterName", SqlDbType.VarChar).Value = c_Name;
                            cmd.Parameters.Add("@RaceID", SqlDbType.Int).Value = c_Race;
                            cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = c_Class;
                            cmd.Parameters.Add("@Level", SqlDbType.Int).Value = c_Level;

                            cmd.Parameters.Add("@Background", SqlDbType.VarChar).Value = c_Background;
                            cmd.Parameters.Add("@Alignment", SqlDbType.VarChar).Value = c_Alignment;
                            cmd.Parameters.Add("@PersonalityTraits", SqlDbType.VarChar).Value = c_PersonalityTraits;
                            cmd.Parameters.Add("@Ideals", SqlDbType.VarChar).Value = c_Ideals;
                            cmd.Parameters.Add("@Bonds", SqlDbType.VarChar).Value = c_Bonds;
                            cmd.Parameters.Add("@Flaws", SqlDbType.VarChar).Value = c_Flaws;
                            cmd.Parameters.Add("@FeaturesTraits", SqlDbType.VarChar).Value = c_FeaturesTraits;

                            cmd.Parameters.Add("@Strength", SqlDbType.Int).Value = c_Strength;
                            cmd.Parameters.Add("@Dexterity", SqlDbType.Int).Value = c_Dexterity;
                            cmd.Parameters.Add("@Constitution", SqlDbType.Int).Value = c_Constitution;
                            cmd.Parameters.Add("@Intelligence", SqlDbType.Int).Value = c_Intelligence;
                            cmd.Parameters.Add("@Wisdom", SqlDbType.Int).Value = c_Wisdom;
                            cmd.Parameters.Add("@Charisma", SqlDbType.Int).Value = c_Charisma;

                            cmd.Parameters.Add("@CurrentHP", SqlDbType.Int).Value = c_CurrentHealth;
                            cmd.Parameters.Add("@TempHP", SqlDbType.Int).Value = c_TemporaryHealth;
                            cmd.Parameters.Add("@AC", SqlDbType.Int).Value = c_ArmorClass;
                            cmd.Parameters.Add("@Initiative", SqlDbType.Int).Value = c_Initiative;
                            cmd.Parameters.Add("@Speed", SqlDbType.Int).Value = c_Speed;
                            cmd.Parameters.Add("@HitDice", SqlDbType.Int).Value = c_HitDice;

                            cmd.Parameters.Add("@StrSave", SqlDbType.Int).Value = c_StrengthSave;
                            cmd.Parameters.Add("@DexSave", SqlDbType.Int).Value = c_DexteritySave;
                            cmd.Parameters.Add("@ConSave", SqlDbType.Int).Value = c_ConstitutionSave;
                            cmd.Parameters.Add("@IntSave", SqlDbType.Int).Value = c_IntelligenceSave;
                            cmd.Parameters.Add("@WisSave", SqlDbType.Int).Value = c_WisdomSave;
                            cmd.Parameters.Add("@ChaSave", SqlDbType.Int).Value = c_CharismaSave;

                            cmd.Parameters.Add("@Acrobatics", SqlDbType.Int).Value = c_Acrobatics;
                            cmd.Parameters.Add("@AnimalHandling", SqlDbType.Int).Value = c_AnimalHandling;
                            cmd.Parameters.Add("@Arcana", SqlDbType.Int).Value = c_Arcana;
                            cmd.Parameters.Add("@Athletics", SqlDbType.Int).Value = c_Athletics; // for some reason this is giving an error???
                            cmd.Parameters.Add("@Deception", SqlDbType.Int).Value = c_Deception;
                            cmd.Parameters.Add("@History", SqlDbType.Int).Value = c_History;
                            cmd.Parameters.Add("@Insight", SqlDbType.Int).Value = c_Insight;
                            cmd.Parameters.Add("@Intimidation", SqlDbType.Int).Value = c_Intimidation;
                            cmd.Parameters.Add("@Investigation", SqlDbType.Int).Value = c_Investigation;
                            cmd.Parameters.Add("@Medicine", SqlDbType.Int).Value = c_Medicine;
                            cmd.Parameters.Add("@Nature", SqlDbType.Int).Value = c_Nature;
                            cmd.Parameters.Add("@Perception", SqlDbType.Int).Value = c_Perception;
                            cmd.Parameters.Add("@Performance", SqlDbType.Int).Value = c_Performance;
                            cmd.Parameters.Add("@Persuasion", SqlDbType.Int).Value = c_Persuasion;
                            cmd.Parameters.Add("@Religion", SqlDbType.Int).Value = c_Religion;
                            cmd.Parameters.Add("@Sleight", SqlDbType.Int).Value = c_SleightOfHand;
                            cmd.Parameters.Add("@Stealth", SqlDbType.Int).Value = c_Stealth;
                            cmd.Parameters.Add("@Survival", SqlDbType.Int).Value = c_Survival;

                            cmd.Parameters.Add("@PassiveWisdom", SqlDbType.Int).Value = c_PassiveWisdom;

                            cmd.Parameters.Add("@CharacterID", SqlDbType.Int).Value = p_CharacterID;

                            cmd.ExecuteNonQuery();

                            Debug.WriteLine("done");
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                //DisplayAlert("Error!", eSql.Message, "OK");
                Debug.WriteLine("Exception: " + eSql.Message);
            }

            Save(this);
        }

        /*
         * Function: EquipSpell
         * Author: Brendon Williams
         * Purpose: Equips Spells similar to weapons to get damage dice
         * last Modified : 2/22/2024 8:15 pm
         */
        public void EquipSpell(int ID, int level, bool doesDmg)
        {
            if (doesDmg) // Have to confirm the table we are looking for
            {
                string query = "SELECT TOP 1 healing FROM dbo.SpellHealing" +
                " WHERE SpellID = @Id AND level >= @Level;";
                Connection connection = Connection.connectionSingleton;
                try
                {
                    using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, EncryptionGrabber.GetEncryptionKey(), EncryptionGrabber.GetEncryptionIV())))
                    {
                        conn.Open();
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = query;

                                // grabs ID from weapon list
                                cmd.Parameters.AddWithValue("@Id", ID);
                                cmd.Parameters.AddWithValue("@Level", level);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        c_damageDice = reader.GetString(0);
                                        string[] dmgSplits = c_damageDice.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //This will handle our +MOD and other + problems
                                        c_damageDice = "";
                                        foreach (var split in dmgSplits)
                                        {
                                            if (split.Contains("d"))
                                            {
                                                //doing nothing here, want to work on structuring + stuff for our dice roller
                                            }
                                            else if (split.Contains("MOD"))
                                            {
                                                dmgSplits[2] = Preferences.Default.Get("IntMod", 0).ToString();
                                            }
                                            else if (split.Contains("+"))
                                            {
                                                dmgSplits[1] = ",+";
                                            }
                                        }
                                        foreach (var split in dmgSplits)
                                        {
                                            c_damageDice += split;
                                        }
                                        c_damageDice = c_damageDice.Replace(" ", "");
                                        c_SEquipped = true;
                                        c_SEquippedID = ID; // These can only be valid if you made it this far (no errors).

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
            else
            {
                string query = "SELECT TOP 1 damage FROM dbo.SpellDamage" +
                " WHERE SpellId = @Id AND level >= @Level;";
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
                                cmd.Parameters.AddWithValue("@Level", level);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        c_damageDice = reader.GetString(0);
                                        if (c_damageDice.Contains("+ MOD"))
                                        {
                                            c_damageDice = c_damageDice.Remove(4); //Use 4 to specifically remove the + MOD
                                        }
                                        else if (c_damageDice.Contains("+ 40"))
                                        {
                                            c_damageDice = c_damageDice.Insert(5, ",");
                                        }
                                        else if (c_damageDice.Contains("+"))
                                        {
                                            c_damageDice = c_damageDice.Insert(4, ",");
                                        }
                                        c_SEquipped = true;
                                        c_SEquippedID = ID;
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

        /*
        * Function: Equip Item
        * Author: Thomas Hewitt
        * Purpose: Equips the specified item to the current character, further modifying their stats.
        * last Modified : 4/13/2024 8:52 am
        */
        public void EquipItem(int ID, int ETypeID)
        {
            if (ETypeID == 0) // Weapon Check
            {
                string query = "SELECT damageDice FROM dbo.Weapon" +
                " WHERE WeaponID = @Id;";
                Connection connection = Connection.connectionSingleton;
                try
                {
                    // Just plugging in the *actual* connection stuff here to remind me to do it everywhere else later for security: Thomas
                    using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, EncryptionGrabber.GetEncryptionKey(), EncryptionGrabber.GetEncryptionIV())))
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
                                        c_damageDice = reader.GetString(0);
                                        c_WEquipped = true;
                                        c_WEquippedID = ID; // These can only be valid if you made it this far (no errors).
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

            if (ETypeID == 1) // Equipment Check
            {
                string query = "SELECT armorClassBase FROM dbo.Gear" +
                " WHERE GearID = @Id;";
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
                                        c_ACBoost = reader.GetInt32(0); // Need to figure out how to add this under the hood, while still being able to unequip gear.
                                        c_EEquipped = true;
                                        c_EEquippedID = ID;
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

        /*
         * Function: RemoveItem
         * Author: Thomas Hewitt
         * Purpose: Unequips the specified item from the current character, negating the stat bonuses it gave.
         * last Modified : 4/21/2024 9:04 am
         */
        public void RemoveItem(int ETypeID)
        {
            switch (ETypeID) // Used a switch because I simply don't like nested if statements :)
            {
                case 0:
                    if (c_WEquipped == true)
                    {
                        c_WEquipped = false;
                        c_WEquippedID = -1;
                        c_damageDice = null;
                    }
                    break;

                case 1:
                    if (c_EEquipped == true)
                    {
                        c_EEquipped = false;
                        c_EEquippedID = -1;
                        c_ACBoost = 0;
                    }
                    break;

                default:
                    // Probably print an error or something.
                    break;
            }
        }

    }
}
