using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace DungeonSidekickMAUI
{
    /*
     * Class: AddSpellViewModel
     * Author: Anthony Rielly
     * Purpose: Create a bindable observable collection that stores UserSpell
     * last Modified: 2/22/2024 by Anthony Rielly
     */
    public class AddSpellViewModel : BindableObject
    {
        private ObservableCollection<Spells> userSpells;

        public ObservableCollection<Spells> UserSpells
        {
            get
            {
                return userSpells;
            }
            set
            {
                // sets the private userSpells to the public UserSpells if they are different
                if (userSpells != value)
                {
                    userSpells = value;
                    OnPropertyChanged("UserSpells");
                }
            }
        }

        /*
         * Class: AddSpellViewModel
         * Author: Anthony Rielly
         * Purpose: Pulls from the DB all of the Spells and populates the observable collection
         * last Modified: 2/22/2024 by Anthony Rielly
         */
        public AddSpellViewModel()
        {
            // creates the new observable collection to add data to
            UserSpells = new ObservableCollection<Spells>
            {

            };

            // hide connection string in the future
            string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
            string query = "SELECT name, SpellId FROM dbo.Spells";

            // open the connection and pull the data
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        // pull from the Spells table
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // create a new user Spell to put in the observable collection for each weapon
                                    var name = new Spells();
                                    name.Name = reader.GetString(0);
                                    name.Id = reader.GetInt32(1);
                                    UserSpells.Add(name);

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
}
