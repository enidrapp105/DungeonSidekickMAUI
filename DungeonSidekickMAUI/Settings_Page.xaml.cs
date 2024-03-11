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

    private void LightTheme(object sender, EventArgs e)
    {
        CustomResources designSave = new CustomResources();

        String Primary = (255 + " " + 255 + " " + 255);
        String Secondary = (190 + " " + 190 + " " + 190);
        String Trinary = (100 + " " + 255 + " " + 255);
        String FC = (0 + " " + 0 + " " + 0);
        String ACCENT = (200 + " " + 200 + " " + 255);
        String ACCESSORY = (50 + " " + 0 + " " + 255);

        designSave.SaveColors(Primary, Secondary, Trinary, FC, ACCENT, ACCESSORY);
        CustomResources.GetColors();

        MainPage(sender, e);
    }
    private void DarkTheme(object sender, EventArgs e)
    {
        CustomResources designSave = new CustomResources();

        String Primary = (0 + " " + 0 + " " + 0);
        String Secondary = (100 + " " + 100 + " " + 100);
        String Trinary = (20 + " " + 20 + " " + 150);
        String FC = (255 + " " + 255 + " " + 255);
        String ACCENT = (200 + " " + 0 + " " + 0);
        // I changed the accessory color
        String ACCESSORY = (0 + " " + 200 + " " + 255);

        designSave.SaveColors(Primary, Secondary, Trinary, FC, ACCENT, ACCESSORY);
        CustomResources.GetColors();

        MainPage(sender, e);
    }

    private void LayoutDesigner(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LayoutDesigner());
    }
}