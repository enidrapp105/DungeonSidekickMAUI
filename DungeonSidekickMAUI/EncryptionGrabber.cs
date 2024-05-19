using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json.Nodes;

namespace DungeonSidekickMAUI
{
    public class EncryptionGrabber
    {
        private static readonly IConfiguration Configuration;

        static EncryptionGrabber()
        {
            Configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        }

        public static string GetConnectionString()
        {
            return Configuration[$"ConnectionString:string"];
        }

        public static string GetEncryptionKey()
        {
            return Configuration["Encryption:Key"];
        }

        public static string GetEncryptionIV()
        {
            return Configuration["Encryption:IV"];
        }
    }
}