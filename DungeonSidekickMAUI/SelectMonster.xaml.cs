namespace DungeonSidekickMAUI;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

/*
* Class: AddToInventory
* Author: Anthony Rielly
* Purpose: Display all of the monsters from the database, the user can search for a specific monster
* last Modified: 2/22/2024 by Anthony Rielly
*/
public partial class SelectMonster : ContentPage
{
    MonsterViewModel monsterViewModel;
    private readonly TimeSpan searchDelay = TimeSpan.FromMilliseconds(500); // Adjust the delay as needed
    private DateTime lastTextChangedTime = DateTime.MinValue;
    public SelectMonster()
    {
        InitializeComponent();
        BindingContext = monsterViewModel = new MonsterViewModel();
    }

    // updates what monsters are visible when the user types something in the search bar
    void Monster_Entry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
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
                        MonsterCollectionView.ItemsSource = monsterViewModel.monsterCollection;
                    else
                        MonsterCollectionView.ItemsSource = monsterViewModel.monsterCollection.Where(i => i.Name.ToLower().StartsWith(e.NewTextValue.ToLower()));
                });
            }
        }, TaskScheduler.Default);
    }

    // Adds the selected item to the inventory class and updates the DB
    private async void ChooseMonster(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Monster mon)
        {
            Monster monster = new Monster();
            monster.AC = mon.AC;
            monster.Id = mon.Id;
            monster.Name = mon.Name;
            monster.HP = mon.HP;
            MonsterSelector.Instance.AddMonster(monster);
            await DisplayAlert("Selected Monster", "Successfully selected the monster for combat.", "Ok");
        }
    }
    private void SelectedMonsters(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MonsterCombat());
    }
}