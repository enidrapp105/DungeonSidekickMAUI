namespace DungeonSidekickMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            DesignStateService designLoad = new DesignStateService();
            DesignAdjust.ChangeDesign(this, designLoad.LoadDesign());
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
