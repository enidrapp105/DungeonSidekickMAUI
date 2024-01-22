namespace DungeonSidekickMAUI;

public partial class LayoutDesigner : ContentPage
{
	public LayoutDesigner()
	{
		InitializeComponent();
	}
    private void SettingsPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Settings_Page());
    }
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var slider = (Slider)sender;
        double value = e.NewValue;

        // Updates the labels based on the slider number
        switch (slider)
        {
            case var _ when slider == redSlider:
                redValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == greenSlider:
                greenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == blueSlider:
                blueValueLabel.Text = value.ToString("F0");
                break;
        }

        // Updates the background color based on the RGB values
        double red = redSlider.Value;
        double green = greenSlider.Value;
        double blue = blueSlider.Value;

        stackLayout.BackgroundColor = Color.FromRgb((int)red, (int)green, (int)blue);
    }

}