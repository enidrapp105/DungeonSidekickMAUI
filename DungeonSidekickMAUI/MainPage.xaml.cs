namespace DungeonSidekickMAUI
{
    public partial class MainPage : ContentPage
    {
        string username;
        public MainPage(string Given_Username)
        {
            username = Given_Username;
            InitializeComponent();
            User_Disp.Text = "Welcome " + username;
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
            Navigation.PushAsync(new Settings_Page(username));
        }
    }

}
