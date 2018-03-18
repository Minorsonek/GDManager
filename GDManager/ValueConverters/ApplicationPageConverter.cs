using System.Diagnostics;
using GDManager.Core;

namespace GDManager
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public static class ApplicationPageConverter
    {
        /// <summary>
        /// Takes a <see cref="ApplicationPage"/> and a view model, if any, and creates the desired page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static BasePage ToBasePage(this ApplicationPage page, object viewModel = null)
        {
            // Find the appropriate page
            switch (page)
            {
                case ApplicationPage.Main:
                    return new MainPage(viewModel as MainPageViewModel);

                case ApplicationPage.Help:
                    return new HelpPage();

                case ApplicationPage.About:
                    return new AboutPage();

                case ApplicationPage.Settings:
                    return new SettingsPage();

                case ApplicationPage.Experimental:
                    return new ExperimentalPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            // Find application page that matches the base page
            if (page is MainPage)
                return ApplicationPage.Main;

            if (page is HelpPage)
                return ApplicationPage.Help;

            if (page is AboutPage)
                return ApplicationPage.About;

            if (page is SettingsPage)
                return ApplicationPage.Settings;

            if (page is ExperimentalPage)
                return ApplicationPage.Experimental;

            // Alert developer of an issue
            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}
