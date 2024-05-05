using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
namespace DungeonSidekickMAUI;

public partial class LayoutDesigner : ContentPage
{
    public LayoutDesigner()
	{
		InitializeComponent();

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, true);
        ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());
    }

    /*
    Author: Jonathan Raffaelly
    Date created: 1/19/24
    Function name: SaveAndReturn
    Purpose: Saves slider values to a string consisting of rgb values to a text document to store thematics.
    Modifications:  1/26/24 - 1/28/24 Overhauled to work with new storage system
                    2/3/24 Made adjustments to handle new color names.

    */
    private void SaveAndReturn(object sender, EventArgs e)
    {
        CustomResources designSave = new CustomResources();
        String Primary = (PredValueLabel.Text + " " + PgreenValueLabel.Text + " " + PblueValueLabel.Text);
        String Secondary = (SredValueLabel.Text + " " + SgreenValueLabel.Text + " " + SblueValueLabel.Text);
        String Trinary = (TredValueLabel.Text + " " + TgreenValueLabel.Text + " " + TblueValueLabel.Text);
        String FC = (FredValueLabel.Text + " " + FgreenValueLabel.Text + " " + FblueValueLabel.Text);
        String Accent = (AccentredValueLabel.Text + " " + AccentgreenValueLabel.Text + " " + AccentblueValueLabel.Text);
        String Accessory = (AccessoryredValueLabel.Text + " " + AccessorygreenValueLabel.Text + " " + AccessoryblueValueLabel.Text);
        designSave.SaveColors(Primary, Secondary, Trinary, FC, Accent, Accessory);
        CustomResources.GetColors();
        Navigation.PushAsync(new Settings_Page());
    }
    private void SettingsPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Settings_Page());
    }
    
    /*
    Author: Jonathan Raffaelly
    Date created: 1/19/24
    Function name: Slider_ValueChanged
    Purpose: Updates colors during runtime for storing for color values. UPDATED: Also updates color example boxes in real time.
    Modifications:  1/27/24 - 1/28/24 Major adjustments to code, added multiple sliders, and color example boxes.
                    2/4/24  Adjusted value names to match new storage descriptions.
    */
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var slider = (Slider)sender;
        double value = e.NewValue;

        // Updates the labels based on the slider number
        switch (slider)
        {
            case var _ when slider == PredSlider:
                PredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == PgreenSlider:
                PgreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == PblueSlider:
                PblueValueLabel.Text = value.ToString("F0");
                break;
        }
        switch (slider)
        {
            case var _ when slider == SredSlider:
                SredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == SgreenSlider:
                SgreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == SblueSlider:
                SblueValueLabel.Text = value.ToString("F0");
                break;
        }
        switch (slider)
        {
            case var _ when slider == TredSlider:
                TredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == TgreenSlider:
                TgreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == TblueSlider:
                TblueValueLabel.Text = value.ToString("F0");
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
        switch (slider)
        {
            case var _ when slider == AccentredSlider:
                AccentredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == AccentgreenSlider:
                AccentgreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == AccentblueSlider:
                AccentblueValueLabel.Text = value.ToString("F0");
                break;
        }
        switch (slider)
        {
            case var _ when slider == AccessoryredSlider:
                AccessoryredValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == AccessorygreenSlider:
                AccessorygreenValueLabel.Text = value.ToString("F0");
                break;
            case var _ when slider == AccessoryblueSlider:
                AccessoryblueValueLabel.Text = value.ToString("F0");
                break;
        }

        // Updates the colors based on the RGB values
        double pred = PredSlider.Value;
        double pgreen = PgreenSlider.Value;
        double pblue = PblueSlider.Value;

        double sred = SredSlider.Value;
        double sgreen = SgreenSlider.Value;
        double sblue = SblueSlider.Value;

        double tred = TredSlider.Value;
        double tgreen = TgreenSlider.Value;
        double tblue = TblueSlider.Value;

        double fred = FredSlider.Value;
        double fgreen = FgreenSlider.Value;
        double fblue = FblueSlider.Value;

        double Accentred = AccentredSlider.Value;
        double Accentgreen = AccentgreenSlider.Value;
        double Accentblue = AccentblueSlider.Value;

        double Accessoryred = AccentredSlider.Value;
        double Accessorygreen = AccentgreenSlider.Value;
        double Accessoryblue = AccentblueSlider.Value;

        // Applies colors to visual boxes for user
        PrimaryBox.BackgroundColor = Color.FromRgb((int)pred, (int)pgreen, (int)pblue);
        SecondaryBox.BackgroundColor = Color.FromRgb((int)sred, (int)sgreen, (int)sblue);
        TrinaryBox.BackgroundColor = Color.FromRgb((int)tred, (int)tgreen, (int)tblue);
        FontBox.BackgroundColor = Color.FromRgb((int)fred, (int)fgreen, (int)fblue);
        AccentBox.BackgroundColor = Color.FromRgb((int)Accentred, (int)Accentgreen, (int)Accentblue);
        AccessoryBox.BackgroundColor = Color.FromRgb((int)Accessoryred, (int)Accessorygreen, (int)Accessoryblue);
    }
}