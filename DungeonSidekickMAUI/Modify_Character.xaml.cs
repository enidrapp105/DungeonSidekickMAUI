namespace DungeonSidekickMAUI;
using Microsoft.Maui.Controls.Shapes;
public partial class Modify_Character : ContentPage
{
    Picker alignmentPicker;
    public List<Editor> entries = new List<Editor>(); // Collection to store references to the dynamically created Entry controls
    ImportedCharacterSheet Char;

    public Modify_Character()
	{
        InitializeComponent();

        // nav bar setup
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, false);
        var customNavBar = cmd.CreateCustomNavigationBar();
        NavigationBar.Children.Add(customNavBar);

        LoadCharacter();
    }
	public void LoadCharacter()
	{
        var hasValue1 = Application.Current.Resources.TryGetValue("PrimaryColor", out object primaryColor);
        var hasValue2 = Application.Current.Resources.TryGetValue("SecondaryColor", out object secondaryColor);
        var hasValue3 = Application.Current.Resources.TryGetValue("TrinaryColor", out object trinaryColor);
        var hasValue4 = Application.Current.Resources.TryGetValue("FontC", out object fontColor);
        var hasValue5 = Application.Current.Resources.TryGetValue("AccentColor", out object accentColor);
        var hasValue6 = Application.Current.Resources.TryGetValue("AccessoryColor", out object accessoryColor);
        Char = ImportedCharacterSheet.Load();

        //Grab all Variables of 
        var variablesFull = typeof(ImportedCharacterSheet).GetProperties();
        int x = 0;
        foreach (var property in variablesFull)
        {
            //Skips properties that should be invisible
            if (!property.CanWrite || property.Name == "p_UID" || property.Name == "p_CharacterID" || property.Name == "c_Equipment" || property.Name == "exists"
                || property.Name == "c_RaceName" || property.Name == "c_ClassName" || property.Name == "c_inv" || property.Name == "c_WEquipped"
                || property.Name == "c_WEquippedID" || property.Name == "c_SEquipped" || property.Name == "c_SEquippedID" || property.Name == "c_damageDice" || property.Name == "c_EEquipped"
                || property.Name == "c_EEquippedID" || property.Name == "c_ACBoost")
            {
                continue;
            }

            //Removes variable prefixes
            var correctVariableName = property.Name.StartsWith("c_") ? property.Name.Substring(2) : property.Name.Substring(2);

            //Create a label for each property
            var newLabel = new Label
            {
                Text = correctVariableName, // Use the property name as label text
                BackgroundColor = (Color)trinaryColor,
                TextColor = (Color)fontColor,
                WidthRequest = 384

            };
            Border frame = new Border
            {
                StrokeThickness = 1,
                Background = (Color)trinaryColor,
                Padding = new Thickness(10, 5),
                HorizontalOptions = LayoutOptions.Center,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(10, 10, 10, 10)
                },
                Stroke = (Color)trinaryColor,
                Content = newLabel
            };
            Sheet.Children.Add(frame);

            if (correctVariableName == "Class")
            {
                Button classChange = new Button()
                {
                    Text = "Class: " + Char.c_ClassName,
                    WidthRequest = 400,
                    HorizontalOptions = LayoutOptions.Center,
                    BackgroundColor = (Color)accentColor,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                classChange.Clicked += GoToClassPickerPage;
                Sheet.Add(classChange);
            }
            else if (correctVariableName == "Race")
            {
                Button raceChange = new Button()
                {
                    Text = "Race: " + Char.c_RaceName,
                    WidthRequest = 400,
                    HorizontalOptions = LayoutOptions.Center,
                    BackgroundColor = (Color)accentColor,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                raceChange.Clicked += GoToRacePickerPage;
                Sheet.Add(raceChange);
            }
            else if (correctVariableName == "Alignment")
            {
                alignmentPicker = new Picker
                {
                    //Title = "Alignment",
                    FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Picker)),
                    BackgroundColor = (Color)Application.Current.Resources["SecondaryColor"],
                    TextColor = (Color)Application.Current.Resources["FontC"],
                    VerticalTextAlignment = TextAlignment.Start,
                    WidthRequest = 400,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(5),
                    TitleColor = (Color)Application.Current.Resources["FontC"]
                };

                // Add items to the Picker
                alignmentPicker.Items.Add("Lawful Good");
                alignmentPicker.Items.Add("Neutral Good");
                alignmentPicker.Items.Add("Chaotic Good");
                alignmentPicker.Items.Add("Lawful Neutral");
                alignmentPicker.Items.Add("True Neutral");
                alignmentPicker.Items.Add("Chaotic Neutral");
                alignmentPicker.Items.Add("Lawful Evil");
                alignmentPicker.Items.Add("Neutral Evil");
                alignmentPicker.Items.Add("Chaotic Evil");

                alignmentPicker.SetBinding(Picker.SelectedItemProperty, new Binding(property.Name, BindingMode.TwoWay, source: Char));

                Sheet.Children.Add(alignmentPicker);
            }
            else
            {
                //Create an editor for each property with a dynamic access name
                var newEntry = new Editor
                {
                    Placeholder = "Enter " + correctVariableName, // Use the property name as entry placeholder
                    BackgroundColor = (Color)secondaryColor,
                    TextColor = (Color)fontColor,
                    WidthRequest = 400,
                    Margin = new Thickness(0, 0, 0, 10),
                    MaxLength = 5
                };
                int[] stringValues = { 0, 2, 3, 4, 5, 6, 7 };
                if (stringValues.Contains(x))
                {
                    newEntry.MaxLength = 50;
                }
                else if (x == 16 || x == 17)
                {
                    newEntry.MaxLength = 4;
                }
                else
                {
                    newEntry.MaxLength = 2;
                }

                //Set a binding between the entry and the corresponding property of the ImportedCharacterSheet instance
                newEntry.SetBinding(Editor.TextProperty, new Binding(property.Name, BindingMode.TwoWay, source: Char));

                
                Sheet.Children.Add(newEntry);
                entries.Add(newEntry); // Add the entry to the collection
                x++;
            }
        }
    }

    private void Save(object sender, EventArgs e)
    {
        List<string> strings = new List<string>();
        List<string> ints = new List<string>();

        strings.Add(entries[0].Text);
        for (int x = 2; x < 8; x++)
            strings.Add(entries[x].Text);
        ints.Add(entries[1].Text);
        for (int y = 8; y < 45; y++)
            ints.Add(entries[y].Text);
        if (!GlobalFunctions.entryCheck(strings, 0))
            return;
        if (!GlobalFunctions.entryCheck(ints, 1))
            return;

        int i = 0;
        ImportedCharacterSheet Char = ImportedCharacterSheet.Load();
        Char.c_Name = entries[i++].Text;
        Char.c_Level = int.Parse(entries[i++].Text);
        Char.c_Background = entries[i++].Text;
        Char.c_Alignment = alignmentPicker.SelectedItem.ToString();
        Char.c_PersonalityTraits = entries[i++].Text;
        Char.c_Ideals = entries[i++].Text;
        Char.c_Bonds = entries[i++].Text;
        Char.c_Flaws = entries[i++].Text;
        Char.c_FeaturesTraits = entries[i++].Text;

        Char.c_Strength = int.Parse(entries[i++].Text);
        Char.c_Dexterity = int.Parse(entries[i++].Text);
        Char.c_Constitution = int.Parse(entries[i++].Text);
        Char.c_Intelligence = int.Parse(entries[i++].Text);
        Char.c_Wisdom = int.Parse(entries[i++].Text);
        Char.c_Charisma = int.Parse(entries[i++].Text);

        Char.c_CurrentHealth = int.Parse(entries[i++].Text);
        Char.c_TemporaryHealth = int.Parse(entries[i++].Text);
        Char.c_ArmorClass = int.Parse(entries[i++].Text);
        Char.c_Initiative = int.Parse(entries[i++].Text);
        Char.c_Speed = int.Parse(entries[i++].Text);
        Char.c_HitDice = int.Parse(entries[i++].Text);

        Char.c_StrengthSave = int.Parse(entries[i++].Text);
        Char.c_DexteritySave = int.Parse(entries[i++].Text);
        Char.c_ConstitutionSave = int.Parse(entries[i++].Text);
        Char.c_IntelligenceSave = int.Parse(entries[i++].Text);
        Char.c_WisdomSave = int.Parse(entries[i++].Text);
        Char.c_CharismaSave = int.Parse(entries[i++].Text);

        Char.c_Acrobatics = int.Parse(entries[i++].Text);
        Char.c_AnimalHandling = int.Parse(entries[i++].Text);
        Char.c_Arcana = int.Parse(entries[i++].Text);
        Char.c_Athletics = int.Parse(entries[i++].Text);
        Char.c_Deception = int.Parse(entries[i++].Text);
        Char.c_History = int.Parse(entries[i++].Text);
        Char.c_Insight = int.Parse(entries[i++].Text);
        Char.c_Intimidation = int.Parse(entries[i++].Text);
        Char.c_Investigation = int.Parse(entries[i++].Text);
        Char.c_Medicine = int.Parse(entries[i++].Text);
        Char.c_Nature = int.Parse(entries[i++].Text);
        Char.c_Perception = int.Parse(entries[i++].Text);
        Char.c_Performance = int.Parse(entries[i++].Text);
        Char.c_Persuasion = int.Parse(entries[i++].Text);
        Char.c_Religion = int.Parse(entries[i++].Text);
        Char.c_SleightOfHand = int.Parse(entries[i++].Text);
        Char.c_Stealth = int.Parse(entries[i++].Text);
        Char.c_Survival = int.Parse(entries[i++].Text);

        Char.c_PassiveWisdom = int.Parse(entries[i++].Text);

        Char.Export();

        //Go back to main page after saving
        Navigation.PushAsync(new MainPage());
    }

    private void Cancel(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }

    private void GoToClassPickerPage(object sender, EventArgs e)
    {
        
        Navigation.PushAsync(new ClassPickerPage(true, Char));
    }

    private void GoToRacePickerPage(object sender, EventArgs e)
    {
        
        Navigation.PushAsync(new RacePickerPage(true, Char));
    }

}