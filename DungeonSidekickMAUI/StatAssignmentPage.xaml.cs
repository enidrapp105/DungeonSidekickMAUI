using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace DungeonSidekickMAUI;

public partial class StatAssignmentPage : ContentPage
{
    private string total1;
    private string total2;
    private string total3;
    private string total4;
    private string total5;
    private string total6;
    private string DEX;
    private string INT;
    private string CHA;
    private string STR;
    private string WIS;
    private string CON;
    private string preferredstat1;
    private string preferredstat2;
    private string preferredstat3;
    private string preferredstat4;
    private string preferredstat5;
    private string preferredstat6;
    private DndClass dndclass;
    CharacterSheet characterSheet;

    public StatAssignmentPage(string total1, string total2, string total3, string total4, string total5, string total6, CharacterSheet characterSheet)
    {
        List<string> totals = new List<string> { total1, total2, total3, total4, total5, total6 };
        List<int> sortedTotals = totals.Select(int.Parse).OrderByDescending(x => x).ToList();
        this.total1 = sortedTotals[0].ToString();
        this.total2 = sortedTotals[1].ToString();
        this.total3 = sortedTotals[2].ToString();
        this.total4 = sortedTotals[3].ToString();
        this.total5 = sortedTotals[4].ToString();
        this.total6 = sortedTotals[5].ToString();
        this.characterSheet = characterSheet;
        InitializeComponent();
        // Convert the totals to integers and sort them
        

        // Set the text of labels with sorted totals
        totallabel1.Text = sortedTotals[0].ToString();
        totallabel2.Text = sortedTotals[1].ToString();
        totallabel3.Text = sortedTotals[2].ToString();
        totallabel4.Text = sortedTotals[3].ToString();
        totallabel5.Text = sortedTotals[4].ToString();
        totallabel6.Text = sortedTotals[5].ToString();
        SetPickerItems();
        this.characterSheet = characterSheet;
    }
    /*
     * Function: SetPickerItems
     * Author: Kenny Rapp
     * Purpose: To set the pickers with the list of stats
     * last Modified: 11/25/2023 7:19pm By Kenny Rapp
     */
    private void SetPickerItems()
    {
        StatPicker1.ItemsSource = GetStatOptions();
        StatPicker2.ItemsSource = GetStatOptions();
        StatPicker3.ItemsSource = GetStatOptions();
        StatPicker4.ItemsSource = GetStatOptions();
        StatPicker5.ItemsSource = GetStatOptions();
        StatPicker6.ItemsSource = GetStatOptions();
    }

    private List<string> GetStatOptions()
    {
        return new List<string> { "DEX", "INT", "CHA", "STR", "WIS", "CON" };
    }
    /*
     * Function: AssignStats
     * Author: Kenny Rapp
     * Purpose: to assign the stats to the integers we generated
     * last Modified: 11/27/2023 12:30am By Kenny Rapp
     */
    private void AssignStats(object sender, EventArgs e)
    {
        StackLayout parent = ((Button)sender).Parent as StackLayout;
        string selectedStat1 = (string)StatPicker1.SelectedItem;
        string selectedStat2 = (string)StatPicker2.SelectedItem;
        string selectedStat3 = (string)StatPicker3.SelectedItem;
        string selectedStat4 = (string)StatPicker4.SelectedItem;
        string selectedStat5 = (string)StatPicker5.SelectedItem;
        string selectedStat6 = (string)StatPicker6.SelectedItem;

        if (HasEmptystring(selectedStat1, selectedStat2, selectedStat3, selectedStat4, selectedStat5, selectedStat6))
        {
            DisplayAlert("Empty Stat(s)", "Please select all stats", "OK");
        }
        else if (HasDuplicates(selectedStat1, selectedStat2, selectedStat3, selectedStat4, selectedStat5, selectedStat6))
        {
            DisplayAlert("Duplicate Stats", "Please select unique stats for each assignment.", "OK");
        }
        else
        {

            totallabel1.Text = $"{selectedStat1} {total1}";
            totallabel2.Text = $"{selectedStat2} {total2}";
            totallabel3.Text = $"{selectedStat3} {total3}";
            totallabel4.Text = $"{selectedStat4} {total4}";
            totallabel5.Text = $"{selectedStat5} {total5}";
            totallabel6.Text = $"{selectedStat6} {total6}";
            AssignValue(totallabel1.Text, totallabel2.Text, totallabel3.Text, totallabel4.Text, totallabel5.Text, totallabel6.Text);
            this.characterSheet.dexterity = DEX;
            this.characterSheet.intelligence = INT;
            this.characterSheet.charisma = CHA;
            this.characterSheet.strength = STR;
            this.characterSheet.wisdom = WIS;
            this.characterSheet.constitution = CON;
            Navigation.PushAsync(new CSheet_Stats(characterSheet));
        }
    }
    /*
     * Function: HasDuplicates
     * Author: Kenny Rapp
     * Purpose: to check if the stats selected have duplicates so the user doesnt assign the same stat twice
     * last Modified: 11/25/2023 7:19pm By Kenny Rapp
     */
    private bool HasDuplicates(params string[] stats)
    {
        HashSet<string> uniqueStats = new HashSet<string>();
        foreach (var stat in stats)
        {
            if (!uniqueStats.Add(stat))
            {
                return true;
            }
        }
        return false;
    }
    /*
     * Function: HasEmptyString
     * Author: Kenny Rapp
     * Purpose: to check if the stats selected have duplicates so the user doesnt assign the same stat twice
     * last Modified: 11/27/2023 12:30am By Kenny Rapp
     */
    private bool HasEmptystring(params string[] stats)
    {
        foreach (var stat in stats)
        {
            if (stat == null)
            {
                return true;
            }
        }
        return false;
    }
    /*
     * Function: AssignValue
     * Author: Kenny Rapp
     * Purpose: to assign the value within the entered string to stat of of the string
     * last Modified: 11/29/2023 3:20pm By Kenny Rapp
     */
    private void AssignValue(params string[] labelname)
    {
        foreach (string stat in labelname)
        {
            string[] sub = stat.Split();
            if (sub.Length == 2)
            {
                // Extract label and value
                string label = sub[0];
                string value = sub[1];

                // Assign value based on the label
                switch (label)
                {
                    case "DEX":
                        DEX = value;
                        break;
                    case "INT":
                        INT = value;
                        break;
                    case "CHA":
                        CHA = value;
                        break;
                    case "STR":
                        STR = value;
                        break;
                    case "WIS":
                        WIS = value;
                        break;
                    case "CON":
                        CON = value;
                        break;
                    default:

                        break;
                }
            }
        }
    }
    private string MapPreferredStatName(int preferredStatValue)
    {
        switch (preferredStatValue)
        {
            case 1:
                return "STR";
            case 2:
                return "DEX";
            case 3:
                return "CON";
            case 4:
                return "INT";
            case 5:
                return "WIS";
            case 6:
                return "CHA";
            default:
                return "UNKNOWN";
        }
    }
    public void AssignStatsbyclass(object sender, EventArgs e)
    {
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";
        string querystring = "SELECT PreferredStat1, PreferredStat2, PreferredStat3, PreferredStat4, PreferredStat5, PreferredStat6 " +
            "FROM dbo.ClassLookup " +
            "WHERE ClassID = @ClassID";
        List<int> preferredstats = new List<int>();
        List<string> preferredstatsstrings = new List<string>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a SqlCommand object with the query string and connection
                using (SqlCommand command = new SqlCommand(querystring, connection))
                {
                    // Add parameters to the command (to prevent SQL injection)
                    command.Parameters.AddWithValue("@ClassID", characterSheet.characterclass);

                    // Open the connection
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                // Read preferred stats from the reader and add them to the list
                                for (int i = 0; i < 6; i++)
                                {
                                    preferredstats.Add(reader.GetInt32(i));
                                }
                            }

                            // Map integer preferred stat values to their corresponding names
                            foreach (int statValue in preferredstats)
                            {
                                preferredstatsstrings.Add(MapPreferredStatName(statValue));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                    }
                }
                AssignPreferredStatsToPickers(preferredstatsstrings);
            }
        }
        catch (Exception eSql)
        {
            DisplayAlert("Error!", eSql.Message, "OK");
            Debug.WriteLine("Exception: " + eSql.Message);
        }
    }
    private void AssignPreferredStatsToPickers(List<string> preferredStats)
    {
        // Iterate through each picker and set its selected item
        for (int i = 0; i < 6; i++)
        {
            string preferredStat = preferredStats[i];
            Picker picker = GetPickerByIndex(i + 1); // Index starts from 1, not 0

            // Find the index of the preferred stat in the picker's items
            int index = picker.Items.IndexOf(preferredStat);

            // Set the selected index of the picker
            picker.SelectedIndex = index;
        }
    }
    private Picker GetPickerByIndex(int index)
    {
        // Find the picker by its name
        return index switch
        {
            1 => StatPicker1,
            2 => StatPicker2,
            3 => StatPicker3,
            4 => StatPicker4,
            5 => StatPicker5,
            6 => StatPicker6,
            _ => null, // Handle error appropriately
        };
    }
}