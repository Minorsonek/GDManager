using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Contains an url which is provided by a user in input box
        /// </summary>
        public string UserUrl { get; set; }

        /// <summary>
        /// TODO: delete
        /// </summary>
        public string OsuAPI { get; set; }

        /// <summary>
        /// List of every saved beatmap
        /// </summary>
        public ObservableCollection<Beatmap> Beatmaps { get; set; } = new ObservableCollection<Beatmap>();

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
            AddNewBeatmapCommand = new RelayCommand(AddBeatmap);
            CheckModsCommand = new RelayCommand(CheckMods);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Adds new beatmap to the list
        /// </summary>
        private void AddBeatmap()
        {
            var osuapi = new Osu.Api(OsuAPI);

            var beatmaps = osuapi.GetBeatmapsAsync(b: 1576795);

            // Create new beatmap object
            var beatmap = new Beatmap
            {
                BeatmapURL = UserUrl,
                BeatmapID = GetBeatmapIDFromURL(UserUrl)
            };

            // Add the beatmap to the list
            Beatmaps.Add(beatmap);
        }

        /// <summary>
        /// Checks every listed beatmap for mods
        /// </summary>
        private void CheckMods()
        {
            // Check each beatmap...
            foreach (var beatmap in Beatmaps)
            {
                // Get the page by beatmap url
                var web = new HtmlWeb();
                var doc = web.Load(beatmap.BeatmapURL);

                // Make it as string
                var outputHtml = doc.DocumentNode.OuterHtml;

                // Check if there is a mod which can be resolved but is not
                if (outputHtml.Contains("\"resolved\":false,\"can_be_resolved\":true"))
                    // New mod is up to be answered
                    beatmap.IsNewMod = true;
                else
                    // No new mods
                    beatmap.IsNewMod = false;
            }

            // Inform the view
            OnPropertyChanged(nameof(Beatmaps));
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Returns the beatmap ID based on it's url
        /// </summary>
        /// <param name="url">The url to the beatmap's thread</param>
        private string GetBeatmapIDFromURL(string url)
        {
            // TODO: implement it
            return string.Empty;
        }

        #endregion
    }
}
