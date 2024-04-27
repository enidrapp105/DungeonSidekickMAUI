using Microsoft.Extensions.Logging;

namespace DungeonSidekickMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //((AppShell)Shell.Current).FlyoutIsPresented = true;
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetTitleView(this, CreateCustomNavigationBar());
            CharacterSheet initCSheet = CharacterSheet.Instance; //Initializes the character sheet, so that we can call the same reference consistently
            User_Disp.Text = "Welcome " + Preferences.Default.Get("Username","");
            AddSelectedChar();
        }

        // Function to create custom navigation bar
        private View CreateCustomNavigationBar()
        {
            // Create a flex layout
            var flexLayout = new FlexLayout
            {
                Direction = Microsoft.Maui.Layouts.FlexDirection.Row,
                JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceBetween,
                AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Center,
                Padding = new Thickness(10, 5)
            };

            // Add elements to the flex layout
            var landingPageButton = new Button
            {
                Text = "Landing Page",
                HorizontalOptions = LayoutOptions.End
            };
            var createButton = new Button
            {
                Text = "New Character Sheet",
                HorizontalOptions = LayoutOptions.End
            };
            var settingsButton = new Button
            {
                Text = "Settings",
                HorizontalOptions = LayoutOptions.End
            };

            flexLayout.Children.Add(landingPageButton);
            flexLayout.Children.Add(createButton);
            flexLayout.Children.Add(settingsButton);
            

            return flexLayout;
        }

        private void AddSelectedChar()
        {
            var hasValue = Application.Current.Resources.TryGetValue("FontC", out object fontColor);
            var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object secondaryColor);
            ImportedCharacterSheet character = ImportedCharacterSheet.Load();
            Label current = new Label();
            current.TextColor = (Color)fontColor;
            current.Text = "Current Character: " + character.c_Name;
            LoadedChar.Add(current);
            Label Level = new Label();
            Level.TextColor = (Color)fontColor;
            Level.Text = "Level: " + character.c_Level;
            LoadedChar.Add(Level);
        }
        private void Create_Character(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Info_For_Stats());
        }
        private void Character_Import(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CSheet_Import());
        }
        private void Modify_Character(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Modify_Character());
        }
        private void Settings_Page(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Settings_Page());
        }
        private void Landing_Page(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LandingPage());
        }

    }
}
