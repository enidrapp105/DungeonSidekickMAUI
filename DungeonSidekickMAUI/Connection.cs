using Microsoft.Extensions.Configuration;
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
            
            connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        }

        // The actual reason this class was made.
        public string? connectionString;
    }
}