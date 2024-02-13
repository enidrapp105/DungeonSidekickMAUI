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
     * Purpose: Handling the intermediate tables for the items table.
     * last Modified: 2/7/2024 by Thomas Hewitt
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
                                    InventoryID = reader.GetInt32(0); // Grabs the ID of the found inventory.
                                }
                                GetIInterStuff();

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
                                    InventoryID = reader.GetInt32(0); // Assigns the freshly generated ID to this inventory for later use.
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
        public void GetIInterStuff() // Query the database and populate the list responsible for storing the data found in the IInter table. Shows your items + quantities.
        {
            string query = "SELECT ItemID, Quantity FROM dbo.IInter" +
            " WHERE InventoryID = @IID";

            IInter.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
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
                            cmd.Parameters.AddWithValue("@IID", InventoryID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                List<int> temp = new List<int>(); // Creating a temporary list to store the individual pieces of data.
                                while (reader.Read()) // Iterate through results of the query
                                {
                                    temp.Clear();
                                    temp.Add(reader.GetInt32(0));
                                    temp.Add(reader.GetInt32(1));
                                    IInter.Add(temp); // Puts the list of data into IInter. This makes UpdateDB easier, in theory.
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

        public void AddItem(int ItemID)
        {
            // Work on this once we get items sorted out.
        }

        public void UpdateDB() // Mass updates the DB with any changes made to this inventory.
        {
            string query = "INSERT INTO dbo.IInter(ItemID, Quantity, InventoryID)" +
            " VALUES(@ItemID, @Quantity, @InventoryID);";

            if(IInter.Count > 0) // Checks if IInter is empty. If not, proceed.
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
                                cmd.Parameters.AddWithValue("@InventoryID", InventoryID);
                                foreach (List<int> stuff in IInter) // Should iterate through every element of IInter and correctly input the necessary data into the query.
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", stuff[0]);
                                    cmd.Parameters.AddWithValue("@Quantity", stuff[1]);
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

        public required int InventoryID { get; set; } // Stores the ID of the current Inventory.

        public required List<List<int>> IInter { get; set; } // Stores the data of the IInter table. Reduces the number of queries we'll need to use.

    }
}
