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
            var createButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "New Character Sheet",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,

            };
            var settingsButton = new Button
            {
                TextColor = (Color)fontColor,
                Text = "Settings",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = (Color)secondaryColor,

            };

            flexLayout.Children.Add(landingPageButton);
            flexLayout.Children.Add(createButton);
            flexLayout.Children.Add(settingsButton);


            return flexLayout;
        }
    }
}
