using Microsoft.IdentityModel.Tokens;
//using UIKit;

namespace DungeonSidekickMAUI;

public partial class CustomResources : ResourceDictionary
{
    public CustomResources()
    {
        InitializeComponent();
    }

    private static string fileName = "DesignSettings.txt";
    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

    public void SaveColors(string BG, string HBG, string FRC, string FC)
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
            Console.WriteLine("Failed to save colors. Error: " + ex);
        }
    }

    public static List<int> LoadColors()
    {

        var numbersList = new List<int>();
        try
        {
            var lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DesignSettings.txt"));
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
                        Console.WriteLine("Failed to get color data.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load colors. Error: " + ex);
        }
        return numbersList;
    }

    public static void GetColors()
    {
        List<int> list = LoadColors();
        var colors = Application.Current.Resources.MergedDictionaries.ToArray()[2]; // need to find a better why to dynamically recall the dictionary

        // Replaces all colors in dictionary with stored values
        if (!colors.IsNullOrEmpty())
        {
            // need to find a better way to handle the number sequences for this, it looks messy
            colors.Remove("BackgroundC");
            colors.Add("BackgroundC", Color.FromRgb(list[0], list[1], list[2]));
            colors.Remove("HeaderC");
            colors.Add("HeaderC", Color.FromRgb(list[3], list[4], list[5]));
            colors.Remove("FrameC");
            colors.Add("FrameC", Color.FromRgb(list[6], list[7], list[8]));
            colors.Remove("FontC");
            colors.Add("FontC", Color.FromRgb(list[9], list[10], list[11]));
        }
    }
}
