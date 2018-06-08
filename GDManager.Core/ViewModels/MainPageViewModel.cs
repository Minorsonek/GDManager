using System.Threading.Tasks;
using System.Windows.Input;

namespace GDManager.Core
{
    /// <summary>
    /// The view model for the main maps page
    /// </summary>
    public class MainPageViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Contains an url to specific beatmap which is provided by user in input box
        /// </summary>
        public string BeatmapUrl { get; set; }

        /// <summary>
        /// Indicating if application is processing beatmaps (is busy)
        /// </summary>
        public bool ProcessingBeatmaps { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to add a new beatmap to the list
        /// </summary>
        public ICommand AddNewBeatmapCommand { get; private set; }

        /// <summary>
        /// The command to check if any mod appeared on listed beatmaps
        /// </summary>
        public ICommand CheckModsCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainPageViewModel()
        {
            // Create commands
            AddNewBeatmapCommand = new RelayCommand(() => Task.Run(async () => await AddUserBeatmap()));
            CheckModsCommand = new RelayCommand(() => Task.Run(async () => await CheckMods()));

            // If there are no beatmaps currently...
            if (IoC.BeatmapManager.Beatmaps.Count == 0)
                // Load saved beatmaps
                Task.Run(async () => await LoadBeatmaps());
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Adds new beatmap to the list based on user's input
        /// </summary>
        private async Task AddUserBeatmap()
        {
            // In the background, add the beatmap to the list based on input
            await RunCommandAsync(() => ProcessingBeatmaps, async () => IoC.BeatmapManager.AddBeatmap(BeatmapWebHelpers.GetBeatmapFromUrl(BeatmapUrl)));

            // Clear url box so its easier to provide next one
            BeatmapUrl = string.Empty;
        }

        /// <summary>
        /// Checks every listed beatmap for mods
        /// </summary>
        private async Task CheckMods()
        {
            // In the background, check for new mods
            await RunCommandAsync(() => ProcessingBeatmaps, async () => BeatmapWebHelpers.CheckMods());
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Loads saved beatmaps from file to the beatmap list
        /// </summary>
        private async Task LoadBeatmaps()
        {
            // In the background, load beatmaps to the list
            await RunCommandAsync(() => ProcessingBeatmaps, async () => IoC.BeatmapManager.LoadBeatmaps());
        }

        #endregion
    }
}
