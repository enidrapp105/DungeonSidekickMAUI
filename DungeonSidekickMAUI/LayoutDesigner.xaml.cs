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
        CustomResources designSave = new CustomResources();
        String BG = (BredValueLabel.Text + " " + BgreenValueLabel.Text + " " + BblueValueLabel.Text);
        String HBG = (HredValueLabel.Text + " " + HgreenValueLabel.Text + " " + HblueValueLabel.Text);
        String FRC = (FRredValueLabel.Text + " " + FRgreenValueLabel.Text + " " + FRblueValueLabel.Text);
        String FC = (FredValueLabel.Text + " " + FgreenValueLabel.Text + " " + FblueValueLabel.Text);
        designSave.SaveColors(BG, HBG, FRC, FC);
        CustomResources.GetColors();
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

        // Updates the colors based on the RGB values
        double bred = BredSlider.Value;
        double bgreen = BgreenSlider.Value;
        double bblue = BblueSlider.Value;

        double hred = HredSlider.Value;
        double hgreen = HgreenSlider.Value;
        double hblue = HblueSlider.Value;

        double frred = FRredSlider.Value;
        double frgreen = FRgreenSlider.Value;
        double frblue = FRblueSlider.Value;

        double fred = FredSlider.Value;
        double fgreen = FgreenSlider.Value;
        double fblue = FblueSlider.Value;

        // Applies colors to visual boxes for user
        BackgroundBox.BackgroundColor = Color.FromRgb((int)bred, (int)bgreen, (int)bblue);
        HeaderBox.BackgroundColor = Color.FromRgb((int)hred, (int)hgreen, (int)hblue);
        FrameBox.BackgroundColor = Color.FromRgb((int)frred, (int)frgreen, (int)frblue);
        FontBox.BackgroundColor = Color.FromRgb((int)fred, (int)fgreen, (int)fblue);
    }


}