using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DungeonSidekickMAUI
{
    /*
     * Class: Inventory
     * Author: Thomas Hewitt
     * Purpose: Handling all tasks related to the inventory table.
     * last Modified: 2/18/2024 by Thomas Hewitt
     */
    public class Inventory
    {
        // Making the string a private member so I don't have to keep instantiating it.
        private string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

        public Inventory(int CharacterID) // Construct the Inventory using the ID of the character it belongs to. Handles the DB nonsense as well.
        {
            // In theory, with the new DB changes, we should only have to call PullItems for the constructor.
            m_CharacterID = CharacterID;
            Items = new List<List<int>>();
            Weapons = new List<List<int>>();
            Equipment = new List<List<int>>();
            //PullItems(); // Should pull what the character currently has in their inventory from the DB.
        }

        // Just clears the lists, used when adding items
        public void ClearItems()
        {
            Items.Clear();
            Weapons.Clear();
            Equipment.Clear();
        }
        public void PullItems() // Query the database and populate the list responsible for storing the data found in the Items table. Shows your items + quantities.
        {
            string query = "SELECT ItemID, Quantity, eTypeID FROM dbo.Inventory" +
            " WHERE CharacterID = @CharacterID";

            Items.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
            Weapons.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
            Equipment.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
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
                                    //temp.Clear();
                                    List<int> temp = new List<int>(); // Creating a temporary list to store the individual pieces of data.
                                    temp.Add(reader.GetInt32(0));
                                    temp.Add(reader.GetInt32(1));
                                    temp.Add(reader.GetInt32(2));

                                    // These if statements will help determine what type of item we are storing. Should store the data in the correct lists.
                                    if (temp[2] == ITEM)
                                    {
                                        Items.Add(temp); // Puts the list of data into Items. This makes UpdateDB easier, in theory.
                                    }
                                    else if (temp[2] == WEAPON)
                                    {
                                        Weapons.Add(temp);
                                    }
                                    else if (temp[2] == EQUIPMENT)
                                    {
                                        Equipment.Add(temp);
                                    }
                                    // If all else fails, do nothing.
                                    else
                                    {
                                        // You done messed up now. How did we get here?
                                    }
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

        // This function should grab an item/weapon/equipment from the appropriate tables in the DB, based on the values brought in.
        public void AddItem(int ItemID, int Quantity, int ETypeID) // Incoming ETypeID is expected to be 0, 1, or 2.
        {
            if(ETypeID < 0 || ETypeID > 2) // Instant fail, someone gave the wrong values.
            {
                return; // Quick exit.
            }
            else // EType was valid, proceed.
            {
                List<int> temp = new List<int>();
                temp.Add(ItemID);
                temp.Add(Quantity);

                // Checking which list to put the data into.
                if (ETypeID == ITEM)
                {
                    Items.Add(temp);
                }

                else if(ETypeID == WEAPON)
                {
                    Weapons.Add(temp);
                }

                else if(ETypeID == EQUIPMENT)
                {
                    Equipment.Add(temp);
                }
            }
        }

        public void RemoveItem(int ItemID, int ETypeID) // Incoming ETypeID is expected to be 0, 1, or 2.
        {
            if (ETypeID < 0 || ETypeID > 2) // Instant fail, someone gave the wrong values.
            {
                return; // Quick exit.
            }
            else // EType was valid, proceed.
            {

                string query = "DELETE FROM dbo.Inventory" +
                    " WHERE ItemID = @ItemID AND eTypeId = @Etype AND CharacterID = @CharacterID);";
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
                                cmd.Parameters.AddWithValue("@Etype", ETypeID);
                                cmd.Parameters.AddWithValue("@ItemID", ItemID);
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

        public void UpdateDB() // Mass updates the DB with any changes made to this inventory.
        {
            string query = "INSERT INTO dbo.Inventory(ItemID, Quantity, eTypeID, CharacterID)" +
            " VALUES(@ItemID, @Quantity, @Etype, @CharacterID);";
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
                            if (Items.Count > 0) // Checks if Items is empty. If not, proceed.
                            {
                                cmd.Parameters.AddWithValue("@Etype", ITEM);
                                foreach (List<int> item in Items) // Should iterate through every element of Items and correctly input the necessary data into the query.
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", item[0]);
                                    cmd.Parameters.AddWithValue("@Quantity", item[1]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            if (Weapons.Count > 0) // Checks if Weapons is empty. If not, proceed.
                            {
                                cmd.Parameters.AddWithValue("@Etype", WEAPON);
                                foreach (List<int> weapon in Weapons)
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", weapon[0]);
                                    cmd.Parameters.AddWithValue("@Quantity", weapon[1]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            if (Equipment.Count > 0) // Checks if Equipment is empty. If not, proceed.
                            {
                                cmd.Parameters.AddWithValue("@Etype", EQUIPMENT);
                                foreach (List<int> equipment in Equipment)
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", equipment[0]);
                                    cmd.Parameters.AddWithValue("@Quantity", equipment[1]);
                                    cmd.ExecuteNonQuery();
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

        //***************************
        // Currently I removed the 'required' from all of these as it would say it must be set in the ctor even though it is.
        //***************************
        public int m_CharacterID { get; set; } // Stores the ID of the current Inventory. This is now tied to the ID of the character.

        public List<List<int>> Items { get; set; } // Stores the data of the Items table. Reduces the number of queries we'll need to use.

        public List<List<int>> Weapons { get; set; } // Stores the data of the Weapons table.

        public List<List<int>> Equipment { get; set; } // Stores the data of the Equipment table.

        // These lable the ETypeID we use in the DB.
        public const int WEAPON = 0;
        public const int EQUIPMENT = 1;
        public const int ITEM = 2;

    }
}
