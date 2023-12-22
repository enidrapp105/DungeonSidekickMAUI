namespace DungeonSidekickMAUI;

public partial class SelectedClassPage : ContentPage
{
    DndClass SelectedClass;
    public SelectedClassPage(DndClass Class)
    {
        SelectedClass = Class;
        InitializeComponent();
        Classlabel.Text = SelectedClass.ClassName;
        ClassDescLabel.Text = SelectedClass.ClassDesc;
        ClassHitDieLabel.Text = "Hit Die:" + SelectedClass.HitDie;
    }
    private void Submit(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CSheet(SelectedClass));
    }

}