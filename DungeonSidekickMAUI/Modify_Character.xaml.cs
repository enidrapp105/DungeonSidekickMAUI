namespace DungeonSidekickMAUI;

public partial class Modify_Character : ContentPage
{
    public List<Entry> entries = new List<Entry>(); // Collection to store references to the dynamically created Entry controls
    public Modify_Character()
	{
		InitializeComponent();
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
        ImportedCharacterSheet Char = ImportedCharacterSheet.Load();

        //Grab all Variables of 
        var variablesFull = typeof(ImportedCharacterSheet).GetProperties();

        foreach (var property in variablesFull)
        {
            //Skips properties that should be invisible
            if (!property.CanWrite || property.Name == "p_UID" || property.Name == "p_CharacterID" || property.Name == "c_Equipment")
            {
                continue;
            }

            //Removes variable prefixes
            var correctVariableName = property.Name.StartsWith("c_") ? property.Name.Substring(2) : property.Name.Substring(2);

            //Create a label for each property
            var newLabel = new Label
            {
                Text = correctVariableName, // Use the property name as label text
                BackgroundColor = (Color)accessoryColor,
                TextColor = (Color)fontColor,
            };

            //Create an entry for each property with a dynamic access name
            var newEntry = new Entry
            {
                Placeholder = "Enter " + correctVariableName, // Use the property name as entry placeholder
                BackgroundColor = (Color)trinaryColor,
                TextColor = (Color)fontColor
            };

            //Set a binding between the entry and the corresponding property of the ImportedCharacterSheet instance
            newEntry.SetBinding(Entry.TextProperty, new Binding(property.Name, BindingMode.TwoWay, source: Char));

            Sheet.Children.Add(newLabel);
            Sheet.Children.Add(newEntry);
            entries.Add(newEntry); // Add the entry to the collection
        }
    }

    private void Save(object sender, EventArgs e)
    {


        int i = 0;
        ImportedCharacterSheet Char = ImportedCharacterSheet.Load();
        Char.c_Name = entries[i++].Text;
        Char.c_Class = int.Parse(entries[i++].Text);
        Char.c_Race = int.Parse(entries[i++].Text);
        Char.c_Level = int.Parse(entries[i++].Text);
        Char.c_Background = entries[i++].Text;
        Char.c_Alignment = entries[i++].Text;
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
        Char.c_Atheletics = int.Parse(entries[i++].Text);
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

}