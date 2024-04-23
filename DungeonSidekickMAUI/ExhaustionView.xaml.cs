namespace DungeonSidekickMAUI;

public partial class ExhaustionView : ContentView
{
    private List<string> exhaustiondescriptions;
    public static readonly BindableProperty LevelProperty = BindableProperty.Create(nameof(Level), typeof(int), typeof(ExhaustionView), 1);

    public int Level
    {
        get => (int)GetValue(LevelProperty);
        set => SetValue(LevelProperty, value);
    }

    public ExhaustionView(List<string> exhaustiondescriptions)
    {
        InitializeComponent();

        // Add levels to the picker
        for (int i = 1; i <= 6; i++)
        {
            LevelPicker.Items.Add(i.ToString());
        }

        // Handle picker selection change
        LevelPicker.SelectedIndexChanged += (sender, e) =>
        {
            int selectedIndex = LevelPicker.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < exhaustiondescriptions.Count)
            {
                DescriptionLabel.Text = exhaustiondescriptions[selectedIndex];
            }
            else
            {
                DescriptionLabel.Text = "";
            }
        };
        this.exhaustiondescriptions = exhaustiondescriptions;
    }


    public event EventHandler RemoveClicked;
}