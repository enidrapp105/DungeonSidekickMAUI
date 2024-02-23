namespace DungeonSidekickMAUI;

public partial class AddToInventory : ContentPage
{
    AddItemViewModel addItemViewModel;
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
            MyCollectionViews.ItemsSource = addItemViewModel.UserItems.Where(i => i.Name.ToLower().Contains(e.NewTextValue.ToLower()));
    }
}