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
            if (!property.CanWrite || property.Name == "p_UID" || property.Name == "p_CharacterID")
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
        string connectionString = "server=satou.cset.oit.edu, 5433; database=harrow; UID=harrow; password=5HuHsW&BYmiF*6; TrustServerCertificate=True; Encrypt=False;";

        string query = "UPDATE dbo.CharacterSheet" +
            " SET CharacterName = @CharacterName, RaceID = @RaceID, ClassID = @ClassID, Level = @Level, Background = @Background, Alignment = @Alignment, PersonalityTraits = @PersonalityTraits, Ideals = @Ideals, Bonds = @Bonds, Flaws = @Flaws," +
            " FeaturesTraits = @FeaturesTraits, Strength = @Strength, Dexterity = @Dexterity, Constitution = @Constitution, Intelligence = @Intelligence, Wisdom = @Wisdom, Charisma = @Charisma, CurrentHP = @CurrentHP, TempHP = @TempHP, AC = @AC, Initiative = @Initiative," +
            " Speed = @Speed, HitDice = @HitDice, StrSave = @StrSave, DexSave = @DexSave, ConSave = @ConSave, IntSave = @IntSave, WisSave = @WisSave, ChaSave = @ChaSave, Acrobatics = @Acrobatics, AnimalHandling = @AnimalHandling, Arcana = @Arcana, Athletics = @Athletics, Deception = @Deception," +
            " History = @History, Insight = @Insight, Intimidation = @Intimidation, Investigation = @Investigation, Medicine = @Medicine, Nature = @Nature, Perception = @Perception, Performance = @Performance, Persuasion = @Persuasion, Religion = @Religion, Sleight = @Sleight, Stealth = @Stealth," +
            " Survival = @Survival, PassiveWisdom = @PassiveWisdom" +
            " WHERE CharacterID = @CharacterID;";





        ImportedCharacterSheet Char = ImportedCharacterSheet.Load();
        Char.c_Name = entries[0].Text;
        Char.c_Class = int.Parse(entries[1].Text);
        Char.c_Race = int.Parse(entries[2].Text);
        Char.c_Level = int.Parse(entries[3].Text);
        Char.c_Background = entries[4].Text;
        Char.c_Alignment = entries[5].Text;


        //Go back to main page after saving
        Navigation.PushAsync(new MainPage());
    }

    private void Cancel(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }

}