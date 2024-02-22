namespace DungeonSidekickMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ((AppShell)Shell.Current).FlyoutIsPresented = true;
            User_Disp.Text = "Welcome " + Preferences.Default.Get("Username","");
        }

        private void Player_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Info_For_Stats());
        }
        private void Player_Import(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CSheet_Import());
        }
        private void Settings_Page(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Settings_Page());
        }
    }

}
