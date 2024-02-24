using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
//using MapKit;
namespace DungeonSidekickMAUI;

public class AddItemViewModel : BindableObject
{
    private ObservableCollection<UserItem> userItems;
    public class UserItem
    {
        public string Name { get; set; }
        public int eTypeId { get; set; }

        public int Id { get; set; }
    }
    public ObservableCollection<UserItem> UserItems
    {
        get
        {
            return userItems;
        }
        set
        {
            if (userItems != value)
            {
                userItems = value;
                OnPropertyChanged("UserItems");
            }
        }
    }


    public AddItemViewModel()
    {

        UserItems = new ObservableCollection<UserItem>
        {
                
        };

        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        string query = "SELECT name, eTypeId, weaponId FROM dbo.Weapon";
        var hasValue = Application.Current.Resources.TryGetValue("FontC", out object fontColor);
        var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object frameColor);
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
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var name = new UserItem();
                                name.Name = reader.GetString(0);
                                name.eTypeId = reader.GetInt32(1);
                                name.Id = reader.GetInt32(2);
                                UserItems.Add(name);
                                
                            }
                        }
                    }
                    query = "SELECT name, eTypeId, armorId FROM dbo.Armor";
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var name = new UserItem();
                                name.Name = reader.GetString(0);
                                name.eTypeId = reader.GetInt32(1);
                                name.Id = reader.GetInt32(2);
                                UserItems.Add(name);
                            }
                        }
                    }
                    query = "SELECT name, eTypeId, gearId FROM dbo.Gear";
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
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
        catch (Exception eSql)
        {
            //DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
        }

    }
}