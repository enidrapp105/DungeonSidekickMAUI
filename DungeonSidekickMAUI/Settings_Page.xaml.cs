namespace DungeonSidekickMAUI;

public partial class Settings_Page : ContentPage
{
    public Settings_Page()
    {
        var customResources = Application.Current.Resources.MergedDictionaries.ToArray()[2];
        var testSection = (Frame)customResources["PageSection"];
        var stack = (StackLayout)testSection.Content;
        var testLabel = new Label
        {
            Text = "Here is an example of the code in use.",
            TextColor = Color.FromRgb(0, 0, 0)
        };
        stack.Children.Add(testLabel);
        var testLabel2 = new Label
        {
            Text = "Here is another example of the code in use.",
            TextColor = Color.FromRgb(0, 0, 0)
        };
        stack.Children.Add(testLabel2);

        InitializeComponent();
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