using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSidekickMAUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        private CharacterSheet CharacterSheetcurrent;
        public LandingPage()
        {
            InitializeComponent();
        }
    }
}