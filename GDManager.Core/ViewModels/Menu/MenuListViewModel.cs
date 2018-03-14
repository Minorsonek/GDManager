using System.Collections.Generic;

namespace GDManager.Core
{
    /// <summary>
    /// The view model for the side menu list with "page-changers"
    /// </summary>
    public class MenuListViewModel : BaseViewModel
    {
        #region Singleton

        /// <summary>
        /// Single instance of this view model
        /// </summary>
        public static MenuListViewModel Instance => new MenuListViewModel();

        #endregion

        #region Public Properties

        /// <summary>
        /// List of items in this menu list
        /// </summary>
        public List<MenuListItemViewModel> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuListViewModel()
        {
            Items = new List<MenuListItemViewModel>
            {
                new MenuListItemViewModel
                {
                    Name = "Main",
                    Icon = IconType.Home,
                    TargetPage = ApplicationPage.Main
                },
                new MenuListItemViewModel
                {
                    Name = "Help",
                    Icon = IconType.InfoCircle,
                    TargetPage = ApplicationPage.Help
                },
                new MenuListItemViewModel
                {
                    Name = "About",
                    Icon = IconType.InfoCircle,
                    TargetPage = ApplicationPage.About
                },
                new MenuListItemViewModel
                {
                    Name = "Experimental",
                    Icon = IconType.InfoCircle,
                    TargetPage = ApplicationPage.Experimental,
                }
            };
        }

        #endregion
    }
}