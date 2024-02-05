using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    /*
     * Class: Inventory
     * Author: Thomas Hewitt
     * Purpose: Handling the intermediate tables for the items, weapons, and equipment tables.
     * last Modified: 2/4/2024 by Thomas Hewitt
     */
    public class Inventory
    {
        public required int WInterID { get; set; } // Stores the ID of the WInter table.

        public required int EInterID { get; set; } // Stores the ID of the EInter table.

        public required int IInterID { get; set; } // Stores the ID of the IInter table.

        public void GetWInterStuff(int IID) // Query the database and present the data from the WInter table. Shows your weapons.
        {
            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
            string query = "SELECT * FROM dbo.WInter" +
                " WHERE InventoryID = @IID";
            // Do something with WInter.
        }

        public void GetEInterStuff(int IID) // Query the database and present the data from the EInter table. Shows your equipment.
        {
            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
            string query = "SELECT * FROM dbo.EInter" +
                " WHERE InventoryID = @IID";
            // Do something with EInter.
        }

        public void GetIInterStuff(int IID) // Query the database and present the data from the IInter table. Shows your items + quantities.
        {
            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
            string query = "SELECT * FROM dbo.IInter" +
                " WHERE InventoryID = @IID";
            // Do something with IInter.
        }
    }
}
