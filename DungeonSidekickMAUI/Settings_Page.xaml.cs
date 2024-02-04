namespace DungeonSidekickMAUI;

public partial class Settings_Page : ContentPage
{
    string username;
	public Settings_Page(string Given_Username)
	{
        username = Given_Username;
		InitializeComponent();
    }
    private void MainPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage(username));
    }
    private void LayoutDesigner(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LayoutDesigner(username));
    }
}