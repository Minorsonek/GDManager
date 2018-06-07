using Ninject;
using System.Collections.ObjectModel;

namespace GDManager.Core
{
    /// <summary>
    /// The base IoC container
    /// </summary>
    public static class IoC
    {
        #region Public Properties

        /// <summary>
        /// The kernel for our IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// The list which contains every listed beatmap
        /// </summary>
        public static ObservableCollection<BeatmapListItemViewModel> Beatmaps { get; set; }

        /// <summary>
        /// A shortcut to access the <see cref="BeatmapManager"/>
        /// </summary>
        public static BeatmapManager BeatmapManager => IoC.Get<BeatmapManager>();

        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel Application => IoC.Get<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="IUIManager"/>
        /// </summary>
        public static IUIManager UI => IoC.Get<IUIManager>();

        #endregion

        #region Construction

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called as soon as our application starts up to ensure all 
        ///       services can be found
        /// </summary>
        public static void Setup()
        {
            // Bind all required view models
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of every listed view model
            // So there is only one instant of listed classes throughout the application
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
            Kernel.Bind<BeatmapManager>().ToConstant(new BeatmapManager());
        }

        #endregion

        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>() => Kernel.Get<T>();
    }
}
