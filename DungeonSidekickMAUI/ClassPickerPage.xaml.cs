using Newtonsoft.Json;
namespace DungeonSidekickMAUI;

public partial class ClassPickerPage : ContentPage
{
    CharacterSheet characterSheet;
    /*
     * Function: ClassPicker default Constructor
     * Author: Kenny Rapp
     * Purpose: Initilizes all of the class choises based on the json file
     * last Modified : 12/04/2023 3:20pm
     */
    public ClassPickerPage(CharacterSheet CharacterSheet)
    {
        InitializeComponent();
        this.characterSheet = CharacterSheet;
        classButtonContainer = new StackLayout()
        {
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            VerticalOptions = LayoutOptions.StartAndExpand
        };

        //couldn't get the json to work
        //C:\Users\Wolfl\source\repos\DungeonSidekick\DungeonSidekick\DungeonSidekick\ClassInfo.json
        //string jsonFilePath = "C:\\Users\\Wolfl\\source\\repos\\DungeonSidekick\\DungeonSidekick\\DungeonSidekick\\ClassInfo.json";
        //string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassInfo.json");
        //so this will do for now
        string jsonData = "[\r\n  {\r\n    \"ClassName\": \"Artificer\",\r\n    \"HitDie\": \"1 d 8\",\r\n    \"ClassDesc\" : \"Artificer Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Barbarian\",\r\n    \"HitDie\": \"1 d 12\",\r\n    \"ClassDesc\": \"Barbarian Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Bard\",\r\n    \"HitDie\": \"1 d 8\",\r\n    \"ClassDesc\": \"Bard Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Cleric\",\r\n    \"HitDie\": \"1 d 8\",\r\n    \"ClassDesc\": \"Cleric Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Druid\",\r\n    \"HitDie\": \"1 d 8\",\r\n    \"ClassDesc\": \"Druid Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Fighter\",\r\n    \"HitDie\": \"1 d 10\",\r\n    \"ClassDesc\": \"Fighter Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Monk\",\r\n    \"HitDie\": \"1 d 8\",\r\n    \"ClassDesc\": \"Monk Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Paladin\",\r\n    \"HitDie\": \"1 d 10\",\r\n    \"ClassDesc\": \"Paladin Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Ranger\",\r\n    \"HitDie\": \"1 d 10\",\r\n    \"ClassDesc\": \"Ranger Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Rogue\",\r\n    \"HitDie\": \"1 d 8\",\r\n    \"ClassDesc\": \"Rogue Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Sorcerer\",\r\n    \"HitDie\": \"1 d 6\",\r\n    \"ClassDesc\": \"Sorcerer Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Warlock\",\r\n    \"HitDie\": \"1 d 8\",\r\n    \"ClassDesc\": \"Warlock Stuff\",\r\n  },\r\n  {\r\n    \"ClassName\": \"Wizard\",\r\n    \"HitDie\": \"1 d 6\",\r\n    \"ClassDesc\": \"Wizard Stuff\",\r\n  }\r\n]";


        var classList = JsonConvert.DeserializeObject<List<DndClass>>(jsonData);


        classButtonContainer = this.FindByName<StackLayout>("classButtonContainer");
        Color color = new Color(255, 0, 0);
        foreach (var dndClass in classList)
        {
            var classButton = new Button
            {
                Text = dndClass.ClassName,
                FontSize = 12,
                CommandParameter = dndClass, // Set the DndClass as a parameter for the command
                HeightRequest = 50,
                WidthRequest = 100,
                MinimumHeightRequest = 50,
                MinimumWidthRequest = 50,
                BackgroundColor = color
            };
            classButton.Clicked += OnClassButtonClicked;

            classButtonContainer.Children.Add(classButton);
        }

        //this.Classpagestack.Children.Add(classButtonContainer);
    }
    /*
     * Function: RollForStats
     * Author: Kenny Rapp
     * Purpose: Navigate to the ClassPicker
     * last Modified : 12/04/2023 3:20pm
     */
    private void OnClassButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button classButton && classButton.CommandParameter is DndClass selectedClass)
        {
            Navigation.PushAsync(new SelectedClassPage(characterSheet, selectedClass));
        }
    }
}