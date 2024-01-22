using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
namespace DungeonSidekickMAUI;

public partial class LayoutDesigner : ContentPage
{
    public LayoutDesigner()
	{
		InitializeComponent();
	}
    private void SaveAndReturn(object sender, EventArgs e)
    {
        DesignStateService designSave = new DesignStateService();
        String BG = (BredValueLabel.Text + " " + BgreenValueLabel.Text + " " + BblueValueLabel.Text);
        String HBG = (HredValueLabel.Text + " " + HgreenValueLabel.Text + " " + HblueValueLabel.Text);
        String FRC = (FRredValueLabel.Text + " " + FRgreenValueLabel.Text + " " + FRblueValueLabel.Text);
        String FC = (FredValueLabel.Text + " " + FgreenValueLabel.Text + " " + FblueValueLabel.Text);
        designSave.SaveDesign(BG, HBG, FRC, FC);
        Navigation.PushAsync(new Settings_Page());
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
            case var _ when slider == BredSlider:
                BredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == BgreenSlider:
                BgreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == BblueSlider:
                BblueValueLabel.Text = value.ToString("F0");
                break;
        }
        switch (slider)
        {
            case var _ when slider == HredSlider:
                HredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == HgreenSlider:
                HgreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == HblueSlider:
                HblueValueLabel.Text = value.ToString("F0");
                break;
        }
        switch (slider)
        {
            case var _ when slider == FRredSlider:
                FRredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == FRgreenSlider:
                FRgreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == FRblueSlider:
                FRblueValueLabel.Text = value.ToString("F0");
                break;
        }
        switch (slider)
        {
            case var _ when slider == FredSlider:
                FredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == FgreenSlider:
                FgreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == FblueSlider:
                FblueValueLabel.Text = value.ToString("F0");
                break;
        }

        // Updates the background color based on the RGB values
        double red = BredSlider.Value;
        double green = BgreenSlider.Value;
        double blue = BblueSlider.Value;

        stackLayout.BackgroundColor = Color.FromRgb((int)red, (int)green, (int)blue);
    }


}