using GDManager.Core;
using System.Windows;

namespace GDManager
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : BasePage<MainPageViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public MainPage(MainPageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            
            // Refresh the UI everytime its needed
            (DataContext as MainPageViewModel).RefreshUI += 
                () => Application.Current.Dispatcher.Invoke
                    (() => (DataContext as MainPageViewModel).RefreshUICommand.Execute(null));
        }

        #endregion
    }
}
