using Microsoft.IdentityModel.Tokens;

namespace DungeonSidekickMAUI;

public partial class CustomResources : ResourceDictionary
{
    public CustomResources()
    {
        InitializeComponent();
    }

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
            Console.WriteLine($"Error loading design: {ex.Message}");
        }
    }

    public static List<int> LoadDesign()
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
                        Console.WriteLine("Failed to get design data.");
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

    public static void GetColors(/*List<int> list*/)
    {
        List<int> list = LoadDesign();
        var colors = Application.Current.Resources.MergedDictionaries.ToArray()[2];
        if (!colors.IsNullOrEmpty())
        {
            //colors.Remove("PrimaryColor");
            //colors.Add("PrimaryColor", new Color(1.0f, 0.0f, 0.0f));
            colors.Remove("BackgroundC");
            colors.Add("BackgroundC", Color.FromRgb(list[0], list[1], list[2]));
            //Application.Current.Resources["BackgroundC"] = Color.FromRgb(list[0], list[1], list[2]);
        }
    }
}
public static class DesignAdjust
{
    public static void ChangeDesign(Page page, List<int> list)
    {
        if (list.IsNullOrEmpty())
        {
            page.BackgroundColor = Color.FromRgb(0, 0, 0);
        }
        else
        {
            int x = 0;
            Color backgroundColor = Color.FromRgb(list[x], list[x + 1], list[x + 2]);
            page.BackgroundColor = backgroundColor;
            x = 3;
        }

    }
}