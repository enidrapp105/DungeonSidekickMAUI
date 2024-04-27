using Microsoft.Extensions.Logging;

namespace DungeonSidekickMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var hasValue2 = Application.Current.Resources.TryGetValue("PrimaryColor", out object primaryColor);
            //((AppShell)Shell.Current).FlyoutIsPresented = true;
            NavigationCommands cmd = new NavigationCommands();
            NavigationPage.SetHasNavigationBar(this, true);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
            NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());
            CharacterSheet initCSheet = CharacterSheet.Instance; //Initializes the character sheet, so that we can call the same reference consistently
            User_Disp.Text = "Welcome " + Preferences.Default.Get("Username","");
            AddSelectedChar();
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
