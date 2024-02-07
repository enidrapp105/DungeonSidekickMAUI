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
     * Purpose: Handling the intermediate tables for the items, weapons, and equipment tables.
     * last Modified: 2/7/2024 by Thomas Hewitt
     */
    public class Inventory
    {
        // Making the string a private member so I don't have to keep instantiating it.
        private string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

        public Inventory(int CharacterID) // Construct the Inventory using the ID of the character it belongs to. Handles the DB nonsense as well.
        {
            string query = "SELECT * FROM dbo.Inventory" +
                " WHERE InventoryID = @IID;";
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
                            int exists = (int)cmd.ExecuteScalar(); // This will check to see if this inventory exists in the database. Returns null if there is no inventory was found.
                            if(exists > 0)
                            {
                                // Grab the information from the DB, inventory exists.
                            }
                            else
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
        public void GetIInterStuff(int IID) // Query the database and present the data from the IInter table. Shows your items + quantities.
        {
            string query = "SELECT * FROM dbo.IInter" +
                " WHERE InventoryID = @IID";
            // Do something with IInter.
        }

        public void UpdateDB()
        {
            // Mass updates the DB with any changes made to this inventory.
        }

        public required int InventoryID { get; set; } // Stores the ID of the current Inventory.

    }
}
