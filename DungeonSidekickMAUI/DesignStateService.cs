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


                var newLines = new List<string>();

                newLines.Add(BG);
                newLines.Add(HBG);
                newLines.Add(FRC);
                newLines.Add(FC);

                File.WriteAllLines(filePath, newLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading state: {ex.Message}");
            }
        }

        public List<int> LoadDesign()
        {
            var numbersList = new List<int>();
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string[] numbersStr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    foreach (string numStr in numbersStr)
                    {
                        if (int.TryParse(numStr, out int num))
                        {
                            numbersList.Add(num);
                        }
                        else
                        {
                            Console.WriteLine($"Failed to parse '{numStr}' as an integer.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading: {ex.Message}");
            }
            return numbersList;
        }
    }
    public static class DesignAdjust
    {
        public static void ChangeDesign(Page page, List<int> list)
        {
            int x = 0;
            Color backgroundColor = Color.FromRgb(list[x], list[x+1], list[x+2]);
            page.BackgroundColor = backgroundColor;
            x = 3;
        }
    }
}