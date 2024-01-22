using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DungeonSidekickMAUI
{
    public class DesignStateService
    {
        private const string FilePath = "DesignSettings.txt";

        public Color? BackgroundColor { get; set; }

        public void SaveState()
        {
            try
            {
                string hexColor = BackgroundColor.ToHex();
                File.WriteAllText(FilePath, hexColor);
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log or display an error message
                Console.WriteLine($"Error saving state: {ex.Message}");
            }
        }

        public void LoadState()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string hexColor = File.ReadAllText(FilePath);
                    BackgroundColor = Color.FromHex(hexColor);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading state: {ex.Message}");
            }
        }
    }
}