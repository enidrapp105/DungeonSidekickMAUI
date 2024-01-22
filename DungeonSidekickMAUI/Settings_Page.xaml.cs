namespace DungeonSidekickMAUI;

public partial class Settings_Page : ContentPage
{
	public Settings_Page()
	{
		InitializeComponent();
	}
    private void MainPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}