using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
namespace DungeonSidekickMAUI;

/*
     * Class: UserItem
     * Author: Anthony Rielly
     * Purpose: For storing multiple pieces of data in the UserItems ObservableCollection below
     * last Modified: 2/22/2024 by Anthony Rielly
     */
public class UserItem
{
    public string Name { get; set; }
    public int eTypeId { get; set; }

    public int Id { get; set; }
}

/*
     * Class: AddItemViewModel
     * Author: Anthony Rielly
     * Purpose: Create a bindable observable collection that stores UserItem
     * last Modified: 2/22/2024 by Anthony Rielly
     */
public class AddItemViewModel : BindableObject
{
    private ObservableCollection<UserItem> userItems;
    
    public ObservableCollection<UserItem> UserItems
    {
        get
        {
            return userItems;
        }
        set
        {
            // sets the private userItems to the public UserItems if they are different
            if (userItems != value)
            {
                userItems = value;
                OnPropertyChanged("UserItems");
            }
        }
    }

    /*
     * Class: AddItemViewModel
     * Author: Anthony Rielly
     * Purpose: Pulls from the DB all of the items and populates the observable collection
     * last Modified: 2/22/2024 by Anthony Rielly
     */
    public AddItemViewModel()
    {
        // creates the new observable collection to add data to
        UserItems = new ObservableCollection<UserItem>
        {
                
        };

        // hide connection string in the future
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        string query = "SELECT name, eTypeId, WeaponId FROM dbo.Weapon";
        
        // open the connection and pull the data
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    // pull from the weapon table
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // create a new user item to put in the observable collection for each weapon
                                var name = new UserItem();
                                name.Name = reader.GetString(0);
                                name.eTypeId = reader.GetInt32(1);
                                name.Id = reader.GetInt32(2);
                                UserItems.Add(name);
                                
                            }
                        }
                    }

                    // pull from the armor table
                    query = "SELECT name, eTypeId, armorId FROM dbo.Armor";
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // create a new user item to put in the observable collection for each armor
                                var name = new UserItem();
                                name.Name = reader.GetString(0);
                                name.eTypeId = reader.GetInt32(1);
                                name.Id = reader.GetInt32(2);
                                UserItems.Add(name);
                            }
                        }
                    }

                    // pull from the gear table
                    query = "SELECT name, eTypeId, GearId FROM dbo.Gear";
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // create a new user item to put in the observable collection for each gear
                                var name = new UserItem();
                                name.Name = reader.GetString(0);
                                name.eTypeId = reader.GetInt32(1);
                                name.Id = reader.GetInt32(2);
                                UserItems.Add(name);
                            }
                        }
                    }
                }
            }
        }
        // if something goes wrong, catch it
        catch (Exception eSql)
        {
            // normally we would have a display alert, but since this is a bindable object class not a page, this is not possible
            // have not come up with a good way to display an error message to the user yet
            Debug.WriteLine("Exception: " + eSql.Message);
        }

    }
}