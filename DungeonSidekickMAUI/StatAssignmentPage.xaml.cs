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
    private DndClass dndclass;
    CharacterSheet characterSheet;

    public StatAssignmentPage(string total1, string total2, string total3, string total4, string total5, string total6, CharacterSheet characterSheet)
    {
        this.total1 = total1;
        this.total2 = total2;
        this.total3 = total3;
        this.total4 = total4;
        this.total5 = total5;
        this.total6 = total6;
        this.characterSheet = characterSheet;
        InitializeComponent();
        totallabel1.Text = total1;
        totallabel2.Text = total2;
        totallabel3.Text = total3;
        totallabel4.Text = total4;
        totallabel5.Text = total5;
        totallabel6.Text = total6;
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
            Navigation.PushAsync(new CSheet(characterSheet));
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
}