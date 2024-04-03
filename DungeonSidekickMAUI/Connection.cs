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
            // Have to get the key and iv from *somewhere.*

            string encryptionKey = EncryptionGrabber.GetEncryptionKey();
            string encryptionIV = EncryptionGrabber.GetEncryptionIV();
            connectionString = Encryption.Encrypt(EncryptionGrabber.GetConnectionString(), encryptionKey, encryptionIV);
        }

        // The actual reason this class was made.
        public string? connectionString;
        public string? encryptionKey;
        public string? encryptionIV;
    }
}