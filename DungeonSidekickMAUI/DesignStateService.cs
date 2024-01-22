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
        private static string fileName = "DesignSettings.txt";
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);


        public void SaveDesign(string BG, string HBG, string FRC, string FC)
        {
            try
            {
                var states = new Dictionary<string, string>
                {
                    { "BackgroundColor", BG },
                    { "HeaderBackgroundColor", HBG },
                    { "FrameColor", FRC },
                    { "FontColor", FC }
                };

                var newLines = new List<string>();
                foreach (var option in states)
                {
                    newLines.Add($"{option.Key}= {option.Value}");
                }
                File.WriteAllLines(filePath, newLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading state: {ex.Message}");
            }
        }

        public void LoadDesign()
        {
            try
            {
                foreach (string line in File.ReadLines(filePath))
                {
    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading: {ex.Message}");
            }
        }
    }
}