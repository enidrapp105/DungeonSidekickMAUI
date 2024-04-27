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
            Application.Current.Resources.TryGetValue("PrimaryColor", out object primaryColor);
            Application.Current.Resources.TryGetValue("FontC", out object fontColor);
            Application.Current.Resources.TryGetValue("SecondaryColor", out object secondaryColor);

            var flexLayout = new FlexLayout
            {
                Direction = Microsoft.Maui.Layouts.FlexDirection.Row,
                JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceBetween,
                AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Center,
                Padding = new Thickness(10, 5),
                BackgroundColor = (Color)primaryColor,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Add elements to the flex layout
            var landingPageButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Landing Page",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,

            };
            landingPageButton.Clicked += Landing_Page;

            var createButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "New Character Sheet",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,

            };
            createButton.Clicked += Create_Character;


            var settingsButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Settings",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,

            };
            settingsButton.Clicked += Settings_Page;
            
            flexLayout.Children.Add(landingPageButton);
            flexLayout.Children.Add(createButton);
            flexLayout.Children.Add(settingsButton);


            return flexLayout;
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
