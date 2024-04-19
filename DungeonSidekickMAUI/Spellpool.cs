using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DungeonSidekickMAUI
{
/*
* Class: Spellpool
* Author: Anthony Rielly
* Purpose: Handling all tasks related to the spellpool.
* last Modified: 04/06/2024 by Anthony Rielly
*/
    public class Spellpool
    {
        private string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        public int m_CharacterID { get; set; } // Stores the ID of the current Inventory. This is now tied to the ID of the character.

        public List<int> Spells { get; set; } // Stores the data of the Spells table. Reduces the number of queries we'll need to use.
        public Spellpool() // Construct the Inventory using the ID of the character it belongs to. Handles the DB nonsense as well.
        {
            m_CharacterID = Preferences.Default.Get("CharacterID", 1); //Calls user ID preference, if it doesn't have one it returns default
            Spells = new List<int>();
            PullSpells(); // Should pull what the character currently has in their inventory from the DB.
        }

        public void ClearSpells()
        {
            Spells.Clear();
        }

        public void PullSpells() // Query the database and populate the list responsible for storing the data found in the Spells table. Shows your Spells + quantities.
        {
            string query = "SELECT SpellID FROM dbo.Spellpool" +
            " WHERE CharacterID = @CharacterID";

            Spells.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
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
                            cmd.Parameters.AddWithValue("@CharacterID", m_CharacterID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {

                                while (reader.Read()) // Iterate through results of the query
                                {
                                    Spells.Add(reader.GetInt32(0)); // Puts the list of data into Spells. This makes UpdateDB easier, in theory.
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

        public void AddSpell(int SpellID) // Incoming ETypeID is expected to be 0, 1, or 2.
        {
            string query = "INSERT INTO dbo.Spellpool(SpellID, CharacterID)" +
            " VALUES(@SpellID, @CharacterID);";
            Spells.Add(SpellID);
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
                            cmd.Parameters.AddWithValue("@CharacterID", m_CharacterID);
                            cmd.Parameters.AddWithValue("@SpellID", SpellID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }

        public void RemoveSpell(int SpellID) // Incoming ETypeID is expected to be 0, 1, or 2.
        {
            string query = "DELETE FROM dbo.Spellpool" +
                " WHERE SpellID = @SpellID AND CharacterID = @CharacterID;";
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
                            cmd.Parameters.AddWithValue("@CharacterID", m_CharacterID);
                            cmd.Parameters.AddWithValue("@SpellID", SpellID);
                            cmd.ExecuteNonQuery();
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
}
