using System.Windows.Controls;
using GDManager.Core;

namespace GDManager
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        public SideMenuControl()
        {
            InitializeComponent();

            // Set the data context to the dedicated view model
            DataContext = new SideMenuViewModel();
        }
    }
}
