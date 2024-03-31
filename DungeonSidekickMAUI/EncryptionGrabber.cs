using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DungeonSidekickMAUI
{
    public class EncryptionGrabber
    {
        private static readonly IConfiguration Configuration;

        static EncryptionGrabber()
        {
            var basePath = AppContext.BaseDirectory; // This allows us to find the file without hardcoding a path in.
            var builder = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
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