using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json.Nodes;
//using Android.Renderscripts;


namespace DungeonSidekickMAUI
{
    public class EncryptionGrabber
    {
        private static readonly IConfiguration Configuration;

        static EncryptionGrabber()
        {
            //var basePath = AppContext.BaseDirectory; // This allows us to find the file without hardcoding a path in.
            //var configFilePath = Path.Combine(basePath, "secrets.json");
            

            //Configuration = builder.Build();
        }

        public static string GetConnectionString()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<EncryptionGrabber>().Build();
            return config[$"ConnectionString:string"];
        }

        public static string GetEncryptionKey()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<EncryptionGrabber>().Build();
            return config["Encryption:Key"];
        }

        public static string GetEncryptionIV()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<EncryptionGrabber>().Build();
            return config["Encryption:IV"];
        }
    }
}