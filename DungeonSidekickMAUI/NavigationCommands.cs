using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    class NavigationCommands
    {
        // Function to create custom navigation bar
        public View CreateCustomNavigationBar()
        {
            // Create a flex layout
            if (!Application.Current.Resources.TryGetValue("PrimaryColor", out object primaryColor) ||
                !Application.Current.Resources.TryGetValue("FontC", out object fontColor) ||
                !Application.Current.Resources.TryGetValue("SecondaryColor", out object secondaryColor))
            {
                Application.Current.MainPage.DisplayAlert("Color Issue", "Our resource dictionary crashed...", "Ok");
                throw new InvalidOperationException("Required resources are not found.");
            }

            // Add elements to the flex layout
            var landingPageButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Landing Page",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,
                WidthRequest = 130
            };
            landingPageButton.Clicked += Landing_Page;

            var createButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "New Sheet",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,
                WidthRequest = 130
            };
            createButton.Clicked += Create_Character;

            var modifyButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Modify Sheet",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,
                WidthRequest = 130
            };
            modifyButton.Clicked += Modify_Character;

            var settingsButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Settings",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,
                WidthRequest = 130
            };
            settingsButton.Clicked += Settings_Page;

            var inventoryButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Inventory",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,
                WidthRequest = 130
            };
            inventoryButton.Clicked += Inventory_Page;

            var spellpoolButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Spellpool",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,
                WidthRequest = 130
            };
            spellpoolButton.Clicked += Spellpool_Page;

            var combatButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Combat",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,
                WidthRequest = 130
            };
            combatButton.Clicked += Combat_Page;

            var changeButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Change Sheet",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,
                WidthRequest = 130
            };
            changeButton.Clicked += Change_Character;
            var grid = new Grid
            {
                RowDefinitions = {
                new RowDefinition { Height = new GridLength(50) },
                new RowDefinition { Height = new GridLength(50) },
                new RowDefinition { Height = new GridLength(50) }
            },
                ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(130) },
                new ColumnDefinition { Width = new GridLength(130) },
                new ColumnDefinition { Width = new GridLength(130) }
            },
                Padding = new Thickness(10, 5),
                WidthRequest = 400,
                BackgroundColor = (Color)primaryColor
            };

            HorizontalStackLayout flexLayout = new HorizontalStackLayout
            {
                Padding = new Thickness(10, 5),
                BackgroundColor = (Color)primaryColor,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            View retVal;
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                flexLayout.Children.Add(landingPageButton);
                flexLayout.Children.Add(createButton);
                flexLayout.Children.Add(changeButton);
                flexLayout.Children.Add(modifyButton);
                flexLayout.Children.Add(inventoryButton);
                flexLayout.Children.Add(spellpoolButton);
                flexLayout.Children.Add(combatButton);
                flexLayout.Children.Add(settingsButton);
                retVal = flexLayout;
            }
            else
            {
                grid.Add(landingPageButton, 0, 0);
                grid.Add(createButton, 0, 1);
                grid.Add(changeButton, 0, 2);
                grid.Add(modifyButton, 1, 0);
                grid.Add(inventoryButton, 1, 1);
                grid.Add(spellpoolButton, 1, 2);
                grid.Add(combatButton, 2, 0);
                grid.Add(settingsButton, 2, 1);
            }
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                retVal = flexLayout;
            }
            else
            {
                retVal = grid;
            }
            return retVal;
        }

        private async void Create_Character(object sender, EventArgs e)
        {
            await MyPushAsync(new Info_For_Stats());
        }
        private async void Character_Import(object sender, EventArgs e)
        {
            await MyPushAsync(new CSheet_Import());
        }
        private async void Modify_Character(object sender, EventArgs e)
        {
            await MyPushAsync(new Modify_Character());
        }
        private async void Settings_Page(object sender, EventArgs e)
        {
            await MyPushAsync(new Settings_Page());
        }
        private async void Landing_Page(object sender, EventArgs e)
        {
            await MyPushAsync(new LandingPage());
        }
        private async void Inventory_Page(object sender, EventArgs e)
        {
            await MyPushAsync(new InventoryPage());
        } 
        private async void Spellpool_Page(object sender, EventArgs e)
        {
            await MyPushAsync(new SpellpoolPage());
        }

        private async void Combat_Page(object sender, EventArgs e)
        {
            await MyPushAsync(new CombatSelector());
        }

        private async void Change_Character(object sender, EventArgs e)
        {
            await MyPushAsync(new CSheet_Import());
        }

        public static async Task MyPushAsync(Page page)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PushAsync(page);
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
        }
    }
}
