namespace DungeonSidekickMAUI;

public partial class Settings_Page : ContentPage
{
	public Settings_Page()
	{
		InitializeComponent();
        DesignStateService designLoad = new DesignStateService();
        DesignAdjust.ChangeDesign(this, designLoad.LoadDesign());
    }
    private void MainPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
    private void LayoutDesigner(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LayoutDesigner());
    }
}