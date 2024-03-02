namespace DungeonSidekickMAUI;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
//using HomeKit;

public partial class AddToInventory : ContentPage
{
    //public int quant = -999;
    AddItemViewModel addItemViewModel;
    private TaskCompletionSource<int> task;
    public AddToInventory()
	{
		InitializeComponent();
        BindingContext = addItemViewModel = new AddItemViewModel();
    }

    void Entry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
            MyCollectionViews.ItemsSource = addItemViewModel.UserItems;
        else
            MyCollectionViews.ItemsSource = addItemViewModel.UserItems.Where(i => i.Name.ToLower().StartsWith(e.NewTextValue.ToLower()));
    }

    private async Task<int> ShowNumberInputPopup()
    {
        var hasValue = Application.Current.Resources.TryGetValue("FontC", out object fontColor);
        var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object frameColor);

        // Create entry for number input
        var numberEntry = new Entry
        {
            Placeholder = "Enter a quantity",
            Keyboard = Keyboard.Numeric,
            WidthRequest = 350,
            TextColor = (Color)fontColor,
            BackgroundColor = (Color)frameColor
        };

        // Create button for submission
        var enterButton = new Button
        {
            Text = "Enter",
            WidthRequest = 350,
            TextColor = (Color)fontColor,
            BackgroundColor = (Color)frameColor
        };

        // Create layout for popup contents
        var layout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Center
        };
        layout.Children.Add(numberEntry);
        layout.Children.Add(enterButton);

        // Create the popup
        var popup = new Popup
        {
            Content = layout
        };

        // TaskCompletionSource to represent waiting for button click
        task = new TaskCompletionSource<int>();

        // Subscribe to button click event
        enterButton.Clicked += async (sender, e) =>
        {
            // Retrieve input number
            if (int.TryParse(numberEntry.Text, out int number))
            {
                // Set the result of the TaskCompletionSource
                task.SetResult(number);
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid number", "OK");
            }

            // Close the popup
            popup.Close();
        };

        // Show the popup
        this.ShowPopup(popup);

        // Wait for the button click and return the entered number
        return await task.Task;
    }

    private async void AddItems(object sender, EventArgs e)
    {

        // Call ShowNumberInputPopup asynchronously and block synchronously until it completes
        int quant = await ShowNumberInputPopup();

        // waiting for quantity to be entered

        Inventory inv = new Inventory(); // HARD CODED TO 0 RIGHT NOW
        if (sender is Button button && button.CommandParameter is UserItem userItem)
        {
            int eTypeId = userItem.eTypeId;
            int id = userItem.Id;
            inv.AddItem(id, quant, eTypeId);
            inv.UpdateDB();
        }
    }
}