namespace DungeonSidekickMAUI;

public partial class Info_For_Stats : ContentPage
{
    public Info_For_Stats()
    {
        InitializeComponent();
    }

    private void To_CSheet(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CSheet());
    }
}