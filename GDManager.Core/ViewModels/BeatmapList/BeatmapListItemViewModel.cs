using System.Diagnostics;
using System.Windows.Input;

namespace GDManager.Core
{
    /// <summary>
    /// Single beatmapset item stored in the list in main page
    /// </summary>
    public class BeatmapListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The id of the beatmap
        /// </summary>
        public string BeatmapID { get; set; }

        /// <summary>
        /// The title (song name, artist)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of this beatmap
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The url to beatmap image website
        /// </summary>
        public string ImageWebsite { get; set; }

        /// <summary>
        /// The star rating of this beatmap
        /// </summary>
        public string StarRating { get; set; }

        /// <summary>
        /// The original discussion url the beatmap was added by
        /// </summary>
        public string DiscussionUrl { get; set; }

        /// <summary>
        /// Indicates if there is new mod to be answered on the beatmap's discussion
        /// </summary>
        public bool IsNewMod { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to open discussion url in browser
        /// </summary>
        public ICommand OpenBeatmapInBrowserCommand { get; private set; }

        /// <summary>
        /// The command to delete beatmap item from the list
        /// </summary>
        public ICommand DeleteBeatmapCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeatmapListItemViewModel()
        {
            // Create commands
            OpenBeatmapInBrowserCommand = new RelayCommand(() => Process.Start(DiscussionUrl));
            DeleteBeatmapCommand = new RelayCommand(() => IoC.BeatmapManager.DeleteBeatmap(this));
        }

        #endregion
    }
}
