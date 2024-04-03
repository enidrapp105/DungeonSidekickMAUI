using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
namespace DungeonSidekickMAUI;

/*
     * Class: Monster
     * Author: Anthony Rielly
     * Purpose: For storing multiple pieces of data in the MonsterCollection ObservableCollection below
     * last Modified: 3/2/2024 by Anthony Rielly
     */
public class Monster
{
    public string Name { get; set; }
    public int Id { get; set; }

    public int AC { get; set; }

    public int HP { get; set; }
}

/*
     * Class: MonsterViewModel
     * Author: Anthony Rielly
     * Purpose: Create a bindable observable collection that stores Monsters
     * last Modified: 3/2/2024 by Anthony Rielly
     */
public class MonsterViewModel : BindableObject
{
    private ObservableCollection<Monster> monsterCol;

    public ObservableCollection<Monster> monsterCollection
    {
        get
        {
            return monsterCol;
        }
        set
        {
            if (monsterCol != value)
            {
                monsterCol = value;
                OnPropertyChanged("MonsterCollection");
            }
        }
    }

    /*
     * Constructor: MonsterViewModel
     * Author: Anthony Rielly
     * Purpose: Pulls from the DB all of the monsters and populates the observable collection
     * last Modified: 3/2/2024 by Anthony Rielly
     */
    public MonsterViewModel()
    {
        // creates the new observable collection to add data to
        monsterCollection = new ObservableCollection<Monster>
        {

        };

        // hide connection string in the future
        Connection connection = Connection.connectionSingleton;
        string query = "SELECT name, monsterId, armor_value,hit_points FROM dbo.Monster";

        // open the connection and pull the data
        try
        {
            using (SqlConnection conn = new SqlConnection(Encryption.Decrypt(connection.connectionString, connection.encryptionKey, connection.encryptionIV)))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    // pull from the monster table
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // create a new monster class to put in the observable collection for each monster
                                var name = new Monster();
                                name.Name = reader.GetString(0);
                                name.Id = reader.GetInt32(1);
                                name.AC = reader.GetInt32(2);
                                name.HP = reader.GetInt32(3);
                                monsterCollection.Add(name);

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