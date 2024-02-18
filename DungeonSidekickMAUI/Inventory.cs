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
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            // Preliminary checks to see if Inventory exists
                            string query = "SELECT InventoryID from dbo.CharacterSheet" +
                            " WHERE CharacterID = @CID;";

                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@CID", CharacterID);
                            int exists = (int)cmd.ExecuteScalar(); // This will check to see if this inventory exists in the database. Returns null if there is no inventory was found.
                            if (exists > 0) // Found it. Grab the information from the DB, inventory exists.
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    m_CharacterID = reader.GetInt32(0); // Grabs the ID of the found inventory.
                                }
                                PullItems();

                            }
                            else // Didn't find it.
                            {
                                // Create a new inventory in the DB, inventory doesn't exist. Then, assign the ID to the member variable.

                                string newQuery = "INSERT INTO dbo.Inventory DEFAULT VALUES;"; // Handy trick, since Inventory only contains a PK, DEFAULT VALUES just increments the ID.
                                cmd.CommandText = newQuery;
                                cmd.ExecuteNonQuery();

                                newQuery = "SELECT InventoryID=IDENT_CURRENT('dbo.Inventory');"; // This will grab the latest InventoryID that was created.
                                cmd.CommandText = newQuery;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    CharacterID = reader.GetInt32(0); // Assigns the freshly generated ID to this inventory for later use.
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
            // Populate the class with data from the database.
        }
        public void PullItems() // Query the database and populate the list responsible for storing the data found in the Items table. Shows your items + quantities.
        {
            string query = "SELECT ItemID, Quantity FROM dbo.Items" +
            " WHERE InventoryID = @IID";

            Items.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
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
                            cmd.Parameters.AddWithValue("@IID", m_CharacterID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                List<int> temp = new List<int>(); // Creating a temporary list to store the individual pieces of data.
                                while (reader.Read()) // Iterate through results of the query
                                {
                                    temp.Clear();
                                    temp.Add(reader.GetInt32(0));
                                    temp.Add(reader.GetInt32(1));
                                    Items.Add(temp); // Puts the list of data into Items. This makes UpdateDB easier, in theory.
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

        public void AddItem(int ItemID, int ETypeID)
        {
            // Work on this once we get items sorted out.
        }

        public void UpdateDB() // Mass updates the DB with any changes made to this inventory.
        {
            string query = "INSERT INTO dbo.Inventory(ItemDetailsID, Quantity, ETypeID, CharacterID)" +
            " VALUES(@ItemID, @Quantity, @Etype, @CharacterID);";

            if (Items.Count > 0) // Checks if Items is empty. If not, proceed.
            {
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
                                cmd.Parameters.AddWithValue("@Etype", ITEM);
                                foreach (List<int> item in Items) // Should iterate through every element of Items and correctly input the necessary data into the query.
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", item[0]);
                                    cmd.Parameters.AddWithValue("@Quantity", item[1]);
                                    cmd.ExecuteNonQuery();
                                }

                                cmd.Parameters.AddWithValue("@Etype", WEAPON);
                                foreach (List<int> weapon in Weapons)
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", weapon[0]);
                                    cmd.Parameters.AddWithValue("@Quantity", weapon[1]);
                                    cmd.ExecuteNonQuery();
                                }

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
                catch (Exception eSql)
                {
                    Debug.WriteLine("Exception: " + eSql.Message);
                }
            }
        }

        public required int m_CharacterID { get; set; } // Stores the ID of the current Inventory. This is now tied to the ID of the character.

        public required List<List<int>> Items { get; set; } // Stores the data of the Items table. Reduces the number of queries we'll need to use.

        public required List<List<int>> Weapons { get; set; } // Stores the data of the Weapons table.

        public required List<List<int>> Equipment { get; set; } // Stores the data of the Equipment table.

        // These lable the ETypeID we use in the DB.
        public const int WEAPON = 0;
        public const int EQUIPMENT = 1;
        public const int ITEM = 2;

    }
}
