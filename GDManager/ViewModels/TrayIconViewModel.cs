using GDManager.Core;
using System.Windows;
using System.Windows.Input;

namespace GDManager
{
    /// <summary>
    /// The view model for minimize tray icon
    /// </summary>
    public class TrayIconViewModel
    {
        #region Commands

        /// <summary>
        /// The command to show main application window
        /// </summary>
        public ICommand ShowMainWindowCommand { get; private set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TrayIconViewModel()
        {
            // Create commands
            ShowMainWindowCommand = new RelayCommand(() => Application.Current.MainWindow.Show());
        }

        #endregion
    }
}
