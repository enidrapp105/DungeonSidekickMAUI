namespace DungeonSidekickMAUI;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using System.Collections.ObjectModel;

/*
* Class: AddToSpellpool
* Author: Anthony Rielly
* Purpose: Display all of the Spells from the database, the user can search for a specific Spell, 
* if they click on it, the Spell will get added to the Spellpool table
* last Modified: 04/06/2024 by Anthony Rielly
*/
public partial class AddToSpellpool : ContentPage
{
    AddSpellViewModel addSpellViewModel;
    private readonly TimeSpan searchDelay = TimeSpan.FromMilliseconds(500); // Adjust the delay as needed
    private DateTime lastTextChangedTime = DateTime.MinValue;
    

    public AddToSpellpool()
    {
        InitializeComponent();

        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands nav = new NavigationCommands();
        Microsoft.Maui.Controls.NavigationPage.SetHasNavigationBar(this, true);
        ((Microsoft.Maui.Controls.NavigationPage)Microsoft.Maui.Controls.Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        Microsoft.Maui.Controls.NavigationPage.SetTitleView(this, nav.CreateCustomNavigationBar());

        BindingContext = addSpellViewModel = new AddSpellViewModel();
    }

    // updates what Spells are visible when the user types something in the search bar
    void Spellpool_Entry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        lastTextChangedTime = DateTime.Now;

        Task.Delay(searchDelay).ContinueWith((task) =>
        {
            // Check if the last text change occurred within the delay period
            if ((DateTime.Now - lastTextChangedTime) >= searchDelay)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (string.IsNullOrWhiteSpace(e.NewTextValue))
                        SpellCollectionView.ItemsSource = addSpellViewModel.UserSpells;
                    else
                        SpellCollectionView.ItemsSource = addSpellViewModel.UserSpells.Where(i => i.Name.ToLower().StartsWith(e.NewTextValue.ToLower()));
                });
            }
        }, TaskScheduler.Default);
    }

    // Adds the selected Spell to the Spellpool class and updates the DB
    private async void AddSpells(object sender, EventArgs e)
    {
        Spellpool inv = new Spellpool();
        inv.PullSpells();
        if (sender is Button button && button.CommandParameter is Spells userSpell)
        {
            int id = userSpell.Id;
            int retVal = inv.AddSpell(id);
            if (retVal == -1)
            {
                await DisplayAlert("Cannot Add Spell", "Your spellpool already contains this spell.", "Ok");
            }
            else if (retVal == -2)
            {
                await DisplayAlert("Cannot Add Spell", "Failed to connect to the database. Please try again.", "Ok");
            }
            else
            {
                await DisplayAlert("Added Spell", "Successfully added to spellpool", "Ok");
            }
        }
    }

    private void GoToLandingFromSpells(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SpellpoolPage());
    }
}