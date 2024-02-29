using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    public class Connection
    {
        private static Connection? connection;

        // Public property to get the singleton instance.
        public static Connection connectionSingleton
        {
            get { return connection ?? (connection = new Connection()); }
        }

        // Private constructor to prevent others from making a new one.
        private Connection()
        {
            try
            {
                connectionString = File.ReadAllText("Conn.txt"); // This should read in the single line of the .txt to construct the connection string.
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("File not found. Please check the file path.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error ocurred: " + ex.Message);
            }
        }

        // The actual reason this class was made.
        public string? connectionString;
    }
}
