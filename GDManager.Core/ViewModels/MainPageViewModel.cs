using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GDManager.Core
{
    /// <summary>
    /// The view model for the main maps page
    /// </summary>
    public class MainPageViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Sublist of every loaded beatmap which rewrites into UI collection as soon as every beatmap is loaded
        /// </summary>
        private List<BeatmapListItemViewModel> mBeatmapsets = new List<BeatmapListItemViewModel>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Contains an url to specific beatmap which is provided by user in input box
        /// </summary>
        public string BeatmapUrl { get; set; }

        /// <summary>
        /// Indicating if application is processing beatmaps (is busy)
        /// </summary>
        public bool ProcessingBeatmaps { get; private set; }

        /// <summary>
        /// An event to fire whenever main UI should refresh itself
        /// </summary>
        public event Action RefreshUI = () => { };

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

        /// <summary>
        /// The command to refresh main UI 
        /// </summary>
        public ICommand RefreshUICommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainPageViewModel()
        {
            // Create commands
            AddNewBeatmapCommand = new RelayCommand(AddUserBeatmap);
            CheckModsCommand = new RelayCommand(() => Task.Run(async () => await CheckMods()));
            RefreshUICommand = new RelayCommand(() =>
            {
                // Rewrite saved beatmaps to the UI collection
                foreach (var beatmap in mBeatmapsets)
                    IoC.Beatmaps.Add(beatmap);
            });

            // Initialize the list
            IoC.Beatmaps = new ObservableCollection<BeatmapListItemViewModel>();

            // Load saved beatmaps
            try
            {
                Task.Run(async () => await LoadBeatmaps());
            }
            catch { }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Adds new beatmap to the list based on user's input
        /// </summary>
        private void AddUserBeatmap()
        {
            try
            {
                // Add the beatmap to the list based on input
                IoC.Beatmaps.AddBeatmap(AddBeatmap(BeatmapUrl));

                // Save current state of beatmaps
                IoC.BeatmapManager.SaveBeatmaps();
            }
            catch { }

            // Clear url box so its easier to provide next one
            BeatmapUrl = string.Empty;
        }

        /// <summary>
        /// Checks every listed beatmap for mods
        /// </summary>
        private async Task CheckMods()
        {
            // Run this in a background...
            await RunCommandAsync(() => ProcessingBeatmaps, async () =>
            {
                // Check each beatmapset...
                foreach (var beatmapset in IoC.Beatmaps)
                {
                    // Get beatmapset discussion 
                    var beatmapDiscussion = GetBeatmapsetDiscussionByURL(beatmapset.DiscussionUrl);

                    // Get into every beatmap discussion
                    foreach (var discussion in beatmapDiscussion.Discussions)
                        // Only if the beatmap is the one we are looking for...
                        if (discussion.BeatmapID == beatmapset.BeatmapID)
                        {
                            // Check if there are mods
                            if (discussion.CanBeResolved && !discussion.IsResolved)
                            {
                                // Mods found, get out of there
                                beatmapset.IsNewMod = true;
                                break;
                            }
                        }
                }
            });
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Adds new beatmap to the list
        /// </summary>
        private BeatmapListItemViewModel AddBeatmap(string difficultyUrl)
        {
            try
            {
                // Convert difficulty url to the beatmapset discussion thread url
                var discussionUrl = ConvertDiffUrlThread(difficultyUrl);

                // Get beatmapset discussion 
                var beatmapDiscussion = GetBeatmapsetDiscussionByURL(discussionUrl);

                // Get the ID of beatmap we are adding
                var id = GetBeatmapIDFromURL(difficultyUrl);

                // Get the beatmap we need from that
                foreach (var tokenBeatmap in beatmapDiscussion.Beatmaps)
                    if (tokenBeatmap.ID == id)
                    {
                        // We have the beatmap we need, return new item based on this
                        return new BeatmapListItemViewModel
                        {
                            DiscussionUrl = discussionUrl,
                            BeatmapID = tokenBeatmap.ID,
                            Title = beatmapDiscussion.Artist + " - " + beatmapDiscussion.Title,
                            Name = tokenBeatmap.DiffName,
                            ImageWebsite = beatmapDiscussion.ImageUrl,
                            StarRating = tokenBeatmap.DiffStarRating
                        };
                    }
            }
            catch { }

            // If none found, TODO: output error
            return null;
        }

        /// <summary>
        /// Converts the difficulty specific url to the beatmapset discussion thread url
        /// </summary>
        private string ConvertDiffUrlThread(string difficultyUrl) => difficultyUrl.Substring(0, difficultyUrl.IndexOf("discussion") + "discussion".Length);

        /// <summary>
        /// Returns the <see cref="BeatmapsetDiscussion"/> object from the osu! website
        /// </summary>
        private BeatmapsetDiscussion GetBeatmapsetDiscussionByURL(string url)
        {
            // Get the page by beatmap url
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // From the html page, take script tags
            var scriptNodes = doc.DocumentNode.Descendants()
                                .Where(n => n.Name == "script");

            // Find the one with json inside
            var jsonString = GetJsonScriptTag(scriptNodes).InnerText;

            // Parse the json
            var parsedJson = JObject.Parse(jsonString);

            // Return beatmapset discussion from that
            return new BeatmapsetDiscussion(parsedJson);
        }

        /// <summary>
        /// Returns the beatmap ID based on it's url
        /// </summary>
        /// <param name="url">The url to the beatmap</param>
        private string GetBeatmapIDFromURL(string url)
        {
            try
            {
                // Split the url by /
                var tokens = url.Split('/');

                // 6th token is our id
                return tokens[6];
            }
            catch
            {
                return null;
            }
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

        /// <summary>
        /// Loads saved beatmaps from file to the list in this page
        /// </summary>
        private async Task LoadBeatmaps()
        {
            // Run this in a background...
            await RunCommandAsync(() => ProcessingBeatmaps, async () =>
            {
                // Load everything from file
                var fileContent = File.ReadAllLines("beatmaps.txt");

                // Convert file content into beatmaps
                for (int i = 0; i < fileContent.Length; i++)
                {
                    // Get url line
                    var difficultyUrl = fileContent[i];

                    // Convert it to beatmap
                    var beatmap = AddBeatmap(difficultyUrl);

                    // Add it to the sublist
                    mBeatmapsets.Add(beatmap);
                }

                // Everything finished, refresh the UI
                RefreshUI.Invoke();
            });
        }

        #endregion
    }
}
