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
     * last Modified: 2/5/2024 by Thomas Hewitt
     */
    public class Inventory
    {
        // Making the string a private member so I don't have to keep instantiating it.
        private string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        public Inventory(int CharacterID) // Construct the Inventory using the ID of the character it belongs to.
        {
            string query = "SELECT * FROM dbo.Inventory" +
                " WHERE InventoryID = @IID";
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
                            cmd.Parameters.AddWithValue("@IID", IID);
                            int exists = (int)cmd.ExecuteScalar(); // This will check to see if this inventory exists in the database. Returns null if there is no inventory attached to this character.
                            if(exists > 0)
                            {
                                // Grab the information from the DB, inventory exists.
                            }
                            else
                            {
                                // Create a new inventory in the DB, inventory doesn't exist.
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

        public required int IID { get; set; } // Stores the ID of the current Inventory.

        //public required int WInterID { get; set; } // Stores the ID of the WInter table.

        //public required int EInterID { get; set; } // Stores the ID of the EInter table.

        public required int IInterID { get; set; } // Stores the ID of the IInter table.

        //public void GetWInterStuff(int IID) // Query the database and present the data from the WInter table. Shows your weapons.
        //{
        //    string query = "SELECT * FROM dbo.WInter" +
        //        " WHERE InventoryID = @IID";
        //    // Do something with WInter.
        //}

        //public void GetEInterStuff(int IID) // Query the database and present the data from the EInter table. Shows your equipment.
        //{
        //    string query = "SELECT * FROM dbo.EInter" +
        //        " WHERE InventoryID = @IID";
        //    // Do something with EInter.
        //}

        public void GetIInterStuff(int IID) // Query the database and present the data from the IInter table. Shows your items + quantities.
        {
            string query = "SELECT * FROM dbo.IInter" +
                " WHERE InventoryID = @IID";
            // Do something with IInter.
        }
    }
}
