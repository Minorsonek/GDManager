using AutoUpdaterDotNET;
using GDManager.Core;
using System.Windows;

namespace GDManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load our IoC and Updater immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup the main application 
            ApplicationSetup();

            // Check for updates
            CheckUpdates();

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Tries to connect with the server and checks if there is new version available for this app
        /// </summary>
        private void CheckUpdates()
        {
            // Get xml file to check if we have newest version
            AutoUpdater.Start("http://minorsonek.pl/gdmanager/releases/GDManager.Update.xml");
        }

        /// <summary>
        /// Configures our application ready for use
        /// </summary>
        private void ApplicationSetup()
        {
            // Setup IoC
            IoC.Setup();

            // Bind a UI Manager
            IoC.Kernel.Bind<IUIManager>().ToConstant(new UIManager());
        }
    }
}
