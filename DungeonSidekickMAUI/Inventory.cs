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
     * last Modified: 4/13/2024 by Thomas Hewitt
     */
    public class Inventory
    {
        //***************************
        // Currently I removed the 'required' from all of these as it would say it must be set in the ctor even though it is.
        //***************************
        public int m_CharacterID { get; set; } // Stores the ID of the current Inventory. This is now tied to the ID of the character.

        public List<List<int>> Items { get; set; } // Need to figure out how I'm going to make this work.

        public List<List<int>> Gear { get; set; } // Stores the data of the Gear table. Reduces the number of queries we'll need to use.

        public List<List<int>> Weapons { get; set; } // Stores the data of the Weapons table.

        public List<List<int>> Equipment { get; set; } // Stores the data of the Equipment table.

        // These lable the ETypeID we use in the DB.
        public const int WEAPON = 0;
        public const int EQUIPMENT = 1;
        public const int GEAR = 2;

        public Inventory() // Construct the Inventory using the ID of the character it belongs to. Handles the DB nonsense as well.
        {
            // In theory, with the new DB changes, we should only have to call PullItems for the constructor.
            m_CharacterID = Preferences.Default.Get("CharacterID", 1); //Calls user ID preference, if it doesn't have one it returns admin default

            Items = new List<List<int>>();
            Gear = new List<List<int>>();
            Weapons = new List<List<int>>();
            Equipment = new List<List<int>>();
            //PullItems(); // Should pull what the character currently has in their inventory from the DB.
        }

        // Just clears the lists, used when adding items
        public void ClearItems()
        {
            Items.Clear();
            Gear.Clear();
            Weapons.Clear();
            Equipment.Clear();
        }
        public void PullItems() // Query the database and populate the list responsible for storing the data found in the Items table. Shows your items + quantities.
        {
            Connection connection = Connection.connectionSingleton;
            string query = "SELECT ItemID, Quantity, eTypeID FROM dbo.Inventory" +
            " WHERE CharacterID = @CharacterID";
            
            Items.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
            Gear.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
            Weapons.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
            Equipment.Clear(); // In case this function gets called incorrectly, clear the list to prepare for receiving data from the DB.
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
                                    if (temp[2] == GEAR)
                                    {
                                        Gear.Add(temp); // Puts the list of data into Items. This makes UpdateDB easier, in theory.
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
                if (ETypeID == GEAR)
                {
                    Gear.Add(temp);
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

        public void RemoveEquipment(int ItemID, int ETypeID) // Incoming ETypeID is expected to be 0, 1, or 2.
        {
            if (ETypeID < 0 || ETypeID > 2) // Instant fail, someone gave the wrong values.
            {
                return; // Quick exit.
            }
            else // EType was valid, proceed.
            {
                switch(ETypeID)
                {
                    case GEAR:
                        for (int i = 0; i < Gear.Count(); ++i)
                        {
                            if (Gear[i][0] == ItemID)
                            {
                                Gear[i].Clear();
                            }
                        }
                        break;

                    case WEAPON:
                        for (int i = 0; i < Weapons.Count(); ++i)
                        {
                            if (Weapons[i][0] == ItemID)
                            {
                                Weapons[i].Clear();
                            }
                        }
                        break;

                    case EQUIPMENT:
                        for (int i = 0; i < Equipment.Count(); ++i)
                        {
                            if (Equipment[i][0] == ItemID)
                            {
                                Equipment[i].Clear();
                            }
                        }
                        break;

                    default:
                        break;
                }
                Connection connection = Connection.connectionSingleton;
                string query = "DELETE FROM dbo.Inventory" +
                    " WHERE ItemID = @ItemID AND eTypeId = @Etype AND CharacterID = @CharacterID;";
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

        // Specifically to remove *items*.
        public void RemoveItem(int ItemID)
        {
            Connection connection = Connection.connectionSingleton;
            string query = "DELETE FROM dbo.Inventory" +
                " WHERE ItemID = @ItemID AND CharacterID = @CharacterID;";
            for(int i = 0; i < Items.Count(); ++i)
            {
                if (Items[i][0] == ItemID)
                {
                    Items[i].Clear(); // Surely this won't cause problems later on.
                }
            }
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
                            cmd.Parameters.AddWithValue("@CharacterID", m_CharacterID);
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

        public void UpdateOneItem(int ItemID, int Quantity, int ETypeID)
        {
            Connection connection = Connection.connectionSingleton;
            string query = "INSERT INTO dbo.Inventory(ItemID, Quantity, eTypeID, CharacterID)" +
            " VALUES(@ItemID, @Quantity, @Etype, @CharacterID);";
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
                            cmd.Parameters.AddWithValue("@CharacterID", m_CharacterID);
                            if (ETypeID == GEAR) // Checks if Gear is empty. If not, proceed.
                            {
                                cmd.Parameters.AddWithValue("@Etype", GEAR);
                            }
                            else if (ETypeID == WEAPON) // Checks if Weapons is empty. If not, proceed.
                            {
                                cmd.Parameters.AddWithValue("@Etype", WEAPON);
                            }
                            else if (ETypeID == EQUIPMENT) // Checks if Equipment is empty. If not, proceed.
                            {
                                cmd.Parameters.AddWithValue("@Etype", EQUIPMENT);
                            }
                            else
                            {
                                // how?
                            }
                            cmd.Parameters.AddWithValue("@ItemID", ItemID);
                            cmd.Parameters.AddWithValue("@Quantity", Quantity);
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
        public void UpdateDB() // Mass updates the DB with any changes made to this inventory.
        {
            Connection connection = Connection.connectionSingleton;
            string query = "INSERT INTO dbo.Inventory(ItemID, Quantity, eTypeID, CharacterID)" +
            " VALUES(@ItemID, @Quantity, @Etype, @CharacterID);";
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
                            cmd.Parameters.AddWithValue("@CharacterID", m_CharacterID);
                            if(Items.Count > 0) // Checks if Items is empty. If not, proceed.
                            {
                                cmd.Parameters.AddWithValue("@Etype", null);
                                foreach (List<int> item in Items) // Should iterate through every element of Items and correctly input the necessary data into the query.
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", item[0]);
                                    cmd.Parameters.AddWithValue("@Quantity", item[1]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            if (Gear.Count > 0) // Checks if Gear is empty. If not, proceed.
                            {
                                cmd.Parameters.AddWithValue("@Etype", GEAR);
                                foreach (List<int> gear in Gear) // Should iterate through every element of Gear and correctly input the necessary data into the query.
                                {
                                    cmd.Parameters.AddWithValue("@ItemID", gear[0]);
                                    cmd.Parameters.AddWithValue("@Quantity", gear[1]);
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

    }
}
