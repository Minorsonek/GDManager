using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ObservableCollection<BeatmapListItem> Beatmaps { get; set; } = new ObservableCollection<BeatmapListItem>();

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

            // Create new beatmap item
            var beatmap = new BeatmapListItem
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

                // From the html page, take script tags
                var scriptNodes = doc.DocumentNode.Descendants()
                                  .Where(n => n.Name == "script");

                // Find the one with json inside
                var jsonString = GetJsonScriptTag(scriptNodes).InnerText;

                // Parse the json
                var parsedJson = JObject.Parse(jsonString);

                // Create beatmapset discussion from that
                var beatmapDiscussion = new BeatmapsetDiscussion(parsedJson);

                // Find the appropriate diff

                // Check if there are mods 

                /*
                var outputHtml = doc.DocumentNode.OuterHtml;

                // Check if there is a mod which can be resolved but is not
                if (outputHtml.Contains("\"resolved\":false,\"can_be_resolved\":true"))
                    // New mod is up to be answered
                    beatmap.IsNewMod = true;
                else
                    // No new mods
                    beatmap.IsNewMod = false;*/
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

        /// <summary>
        /// Returns the script html tag with json inside
        /// </summary>
        /// <param name="scriptNodes">Script nodes to check</param>
        private HtmlNode GetJsonScriptTag(IEnumerable<HtmlNode> scriptNodes)
        {
            // Check each node
            foreach (var node in scriptNodes)
                // Check if json is inside, based on "beatmapset" json tag which starts every json discuss
                if (node.InnerText.Contains("\"beatmapset\""))
                    return node;

            return null;
        }

        #endregion
    }
}
