namespace DungeonSidekickMAUI;

public partial class SelectedClassPage : ContentPage
{
    public DndClass SelectedClass;
    CharacterSheet characterSheet;
    public SelectedClassPage(CharacterSheet characterSheet, DndClass selectedclass)
    {
        this.characterSheet = characterSheet;
        this.SelectedClass = selectedclass;
        InitializeComponent();
        Classlabel.Text = SelectedClass.ClassName;
        ClassDescLabel.Text = SelectedClass.ClassDesc;
        ClassHitDieLabel.Text = "Hit Die:" + SelectedClass.HitDie;
    }
    private void Submit(object sender, EventArgs e)
    {
        characterSheet.characterclass = SelectedClass;
        Navigation.PushAsync(new CSheet(characterSheet));
    }

}