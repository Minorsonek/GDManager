﻿using GDManager.Core;
using System.Collections.ObjectModel;

namespace GDManager
{
    /// <summary>
    /// Locates view models from the IoC for use in binding in Xaml files
    /// </summary>
    public class ViewModelLocator
    {
        #region Public Properties

        /// <summary>
        /// Singleton instance of the locator
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// The application view model
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => IoC.Application;

        /// <summary>
        /// The main beatmap list
        /// </summary>
        public static ObservableCollection<BeatmapListItemViewModel> Beatmaps => IoC.Beatmaps;

        #endregion
    }
}
