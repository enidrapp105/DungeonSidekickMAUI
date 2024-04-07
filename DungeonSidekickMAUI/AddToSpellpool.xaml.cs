namespace DungeonSidekickMAUI;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

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

    public AddToSpellpool()
    {
        InitializeComponent();
        BindingContext = addSpellViewModel = new AddSpellViewModel();
    }

    // updates what Spells are visible when the user types something in the search bar
    void Entry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
            SpellCollectionView.ItemsSource = addSpellViewModel.UserSpells;
        else
            SpellCollectionView.ItemsSource = addSpellViewModel.UserSpells.Where(i => i.Name.ToLower().StartsWith(e.NewTextValue.ToLower()));
    }

    // Adds the selected Spell to the Spellpool class and updates the DB
    private async void AddSpells(object sender, EventArgs e)
    {
        Spellpool inv = new Spellpool();
        if (sender is Button button && button.CommandParameter is Spells userSpell)
        {
            int id = userSpell.Id;
            inv.AddSpell(id);
        }
    }

    private void GoToLandingFromSpells(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LandingPage());
    }
}