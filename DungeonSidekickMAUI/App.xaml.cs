namespace DungeonSidekickMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CustomResources.GetColors();
            MainPage = new AppShell();
        }
    }
}
