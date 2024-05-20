namespace DungeonSidekickMAUI;

using Microsoft.Data.SqlClient;

public partial class ExhaustionView : ContentView
{
    private List<string> exhaustiondescriptions;
    public static readonly BindableProperty LevelProperty = BindableProperty.Create(nameof(Level), typeof(int), typeof(ExhaustionView), 1);
    Connection connection = Connection.connectionSingleton;
    int CharacterID;
    int exhaustionlevel;

    public int Level
    {
        get => (int)GetValue(LevelProperty);
        set => SetValue(LevelProperty, value);
    }

    public ExhaustionView(List<string> exhaustiondescriptions, int characterid)
    {
        exhaustionlevel = -1;
        CharacterID = characterid;
        InitializeComponent();

        // nav bar setup
        Color primaryColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"];
        NavigationCommands cmd = new NavigationCommands();
        NavigationPage.SetHasNavigationBar(this, true);
        ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)primaryColor;
        NavigationPage.SetTitleView(this, cmd.CreateCustomNavigationBar());

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
                addleveltodatabase(selectedIndex, CharacterID);
            }
            else
            {
                DescriptionLabel.Text = "";
            }
        };
        this.exhaustiondescriptions = exhaustiondescriptions;
        using (SqlConnection conn = new SqlConnection(connection.connectionString))
        {
            string query = "SELECT ExaustionLevel FROM CharacterExhaustionLevelLookup WHERE CharacterID = @CharacterID;";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@CharacterID", CharacterID);
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {
                // No need to create another SqlCommand here
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    LevelPicker.SelectedIndex = Convert.ToInt32(result);
                }
            }
        }
    }
    void addleveltodatabase(int exhaustionLevel, int characterID)
    {
        int existingexhaustionlevel = -1;
        using (SqlConnection conn = new SqlConnection(connection.connectionString))
        {
            string query = "SELECT ExaustionLevel FROM CharacterExhaustionLevelLookup WHERE CharacterID = @CharacterID;";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@CharacterID", characterID);
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {
                // No need to create another SqlCommand here
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    existingexhaustionlevel = Convert.ToInt32(result);
                }
            }
            if (existingexhaustionlevel == -1)
            {
                query = "INSERT INTO CharacterExhaustionLevelLookup (CharacterID, ExaustionLevel) VALUES (@CharacterID, @ExaustionLevel);";
                command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CharacterID", characterID);
                command.Parameters.AddWithValue("@ExaustionLevel", exhaustionLevel);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                query = "DELETE FROM CharacterExhaustionLevelLookup WHERE CharacterID = @CharacterID;";
                command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CharacterID", characterID);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    command.ExecuteNonQuery();
                }
                query = "INSERT INTO CharacterExhaustionLevelLookup (CharacterID, ExaustionLevel) VALUES (@CharacterID, @ExaustionLevel);";
                command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@CharacterID", characterID);
                command.Parameters.AddWithValue("@ExaustionLevel", exhaustionLevel);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    command.ExecuteNonQuery();
                }
            }
        }

    }

    public event EventHandler RemoveClicked;
}