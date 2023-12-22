namespace DungeonSidekickMAUI;

public partial class RollForStatsPage : ContentPage
{
    private Random random = new Random();
    private DndClass dndclass;
    public RollForStatsPage(DndClass selectedclass)
    {
        dndclass = selectedclass;
        InitializeComponent();
    }
    /*
     * Function: OnButtonClicked
     * Author: Kenny Rapp
     * Purpose: When one of the buttons are clicked it rerolls the number inside of the button
     * last Modified: 11/28/2023 9:21pm by Kenny Rapp
     */
    private void OnButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        StackLayout row = button.Parent as StackLayout;
        Label totalLabel = this.FindByName<Label>($"total{row.ClassId}");

        if (row != null && totalLabel != null)
        {
            Reroll(button, row, totalLabel);
        }
    }
    /*
     * Function: OnRollButtonClicked
     * Author: Kenny Rapp
     * Purpose: On click of the roll button it rerolls all of the buttons in that row
     * last Modified: 11/28/2023 9:21pm by Kenny Rapp
     */
    private void OnRollButtonClicked(object sender, EventArgs e)
    {
        Button rollButton = (Button)sender;
        StackLayout row = rollButton.Parent as StackLayout;
        Label totalLabel = this.FindByName<Label>($"total{row.ClassId}");


        if (row != null && totalLabel != null)
        {
            int total = 0;

            foreach (View view in row.Children)
            {
                if (view is Button button)
                {
                    if (button.Text != "Roll")
                    {
                        total += Reroll(button, row, totalLabel);
                    }
                }
            }
            //if (stackLayout.Children.LastOrDefault() is Label totalLabel)
            //{
            //    totalLabel.Text = $"Total: {total}";
            //}
        }
    }
    /*
     * Function: Reroll
     * Author: Kenny Rapp
     * Purpose: picks a number between 1 and 6 and assigns that number to the buttun text
     * last Modified: 11/19/2023 5:42pm
     */
    private int Reroll(Button button, StackLayout row, Label oldtotal)
    {
        int randomNumber = random.Next(1, 7);
        button.Text = randomNumber.ToString();
        UpdateTotal(row, oldtotal);
        return randomNumber;
    }
    /*
     * Function: UpdateTotal
     * Author: Kenny Rapp
     * Purpose: Update total label with new total
     * last Modified: 11/28/2023 9:21pm by Kenny Rapp
     */
    private void UpdateTotal(StackLayout row, Label oldtotal)
    {
        int newtotal = 0;
        List<int> list = new List<int>();
        int droppedstat;
        int rowNumber = int.Parse(row.ClassId);

        foreach (View view in row.Children)
        {
            if (view is Button button && button.Text != "Roll" && button.Text != "")
            {
                list.Add(int.Parse(button.Text));
            }
        }
        list.Sort();
        droppedstat = list[0];
        list.Remove(list[0]);
        foreach (int stat in list)
        {
            newtotal += stat;
        }
        Label totalLabel = this.FindByName<Label>($"total{rowNumber}");
        if (totalLabel != null)
        {
            totalLabel.Text = $"Total: {newtotal} - Dropped: {droppedstat}";
        }

    }
    /*
     * Function: ExtractIntFromTotal
     * Author: Kenny Rapp
     * Purpose: to get the number from the total label without "Total:"
     * last Modified: 11/25/2023 7:19pm By Kenny Rapp
     */
    private string ExtractIntFromTotal(string total)
    {
        string[] subs = total.Split();

        return subs[1];
    }
    /*
     * Function: SubmitStats
     * Author: Kenny Rapp
     * Purpose: to navigate to the next page withe the stats calculated on this page
     * last Modified: 11/25/2023 7:19pm By Kenny Rapp
     */
    private void SubmitStats(object sender, EventArgs e)
    {

        //StackLayout stackLayout = ((Button)sender).Parent as StackLayout;

        StackLayout parentstack = ((Button)sender).Parent as StackLayout;
        if (AllButtonsPopulated(parentstack))
        {
            string retotal1 = ExtractIntFromTotal(total1.Text);
            string retotal2 = ExtractIntFromTotal(total2.Text);
            string retotal3 = ExtractIntFromTotal(total3.Text);
            string retotal4 = ExtractIntFromTotal(total4.Text);
            string retotal5 = ExtractIntFromTotal(total5.Text);
            string retotal6 = ExtractIntFromTotal(total6.Text);
            Navigation.PushAsync(new StatAssignmentPage(retotal1, retotal2, retotal3, retotal4, retotal5, retotal6, dndclass));
        }
        else
        {
            DisplayAlert("Empty Stat(s)", "Please roll all stats", "OK");
        }
    }
    /*
     * Function: AllButtonsPopulated
     * Author: Kenny Rapp
     * Purpose: to check if all of the buttons have a number in them
     * last Modified: 11/27/2023 2:19pm By Kenny Rapp
     */
    private bool AllButtonsPopulated(StackLayout parentstack)
    {
        foreach (View view in parentstack.Children)
        {
            if (view is StackLayout)
            {
                foreach (View childView in ((StackLayout)view).Children)
                {
                    if (childView is Button && ((Button)childView).Text != "Roll")
                    {
                        if (string.IsNullOrEmpty(((Button)childView).Text))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
}