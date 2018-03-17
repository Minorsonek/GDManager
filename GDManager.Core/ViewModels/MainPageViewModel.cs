using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        /// Contains an discussion url which is provided by a user in input box
        /// </summary>
        public string DiscussionUrl { get; set; }

        /// <summary>
        /// Contains an url to specific beatmap which is provided by user in input box
        /// </summary>
        public string BeatmapUrl { get; set; }

        /// <summary>
        /// List of every saved beatmap
        /// </summary>
        public ObservableCollection<BeatmapListItem> Beatmapsets { get; set; } 

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
            AddNewBeatmapCommand = new RelayCommand(AddUserBeatmap);
            CheckModsCommand = new RelayCommand(CheckMods);

            // Load saved beatmaps
            LoadBeatmaps();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Adds new beatmap to the list based on user's input
        /// </summary>
        private void AddUserBeatmap()
        {
            // Add the beatmap to the list based on input
            AddBeatmap(DiscussionUrl, BeatmapUrl);

            // Save current state of beatmaps
            SaveBeatmaps();
        }

        /// <summary>
        /// Checks every listed beatmap for mods
        /// </summary>
        private void CheckMods()
        {
            // Check each beatmapset...
            foreach (var beatmapset in Beatmapsets)
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
                            beatmapset.IsNewMod = true;
                        else
                            beatmapset.IsNewMod = false;
                    }
            }

            // Inform the view
            OnPropertyChanged(nameof(Beatmapsets));
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Adds new beatmap to the list
        /// </summary>
        private void AddBeatmap(string discussionUrl, string difficultyUrl)
        {
            // Get beatmapset discussion 
            var beatmapDiscussion = GetBeatmapsetDiscussionByURL(discussionUrl);

            // Get the ID of beatmap we are adding
            var id = GetBeatmapIDFromURL(difficultyUrl);

            // Get the beatmap we need from that
            foreach (var tokenBeatmap in beatmapDiscussion.Beatmaps)
                if (tokenBeatmap.ID == id)
                {
                    // We have the beatmap we need, create new item based on this
                    var beatmap = new BeatmapListItem
                    {
                        DiscussionUrl = discussionUrl,
                        BeatmapID = tokenBeatmap.ID,
                        Title = beatmapDiscussion.Artist + " - " + beatmapDiscussion.Title,
                        Name = tokenBeatmap.DiffName,
                        StarRating = tokenBeatmap.DiffStarRating
                    };

                    // Add the beatmap to the list
                    Beatmapsets.Add(beatmap);
                }
        }

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
            // Split the url by /
            var tokens = url.Split('/');

            // 4th token is our id
            return tokens[4];
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
        private void LoadBeatmaps()
        {
            // Initialize the list
            Beatmapsets = new ObservableCollection<BeatmapListItem>();

            // Load everything from file
            var fileContent = File.ReadAllLines("beatmaps.txt");

            // Convert file content into beatmaps
            for (int i = 0; i < fileContent.Length; i += 2)
            {
                // Get two lines
                var discussionUrl = fileContent[i];
                var difficultyUrl = fileContent[i + 1];

                // Add the beatmap based on that
                AddBeatmap(discussionUrl, difficultyUrl);
            }
        }

        /// <summary>
        /// Saves current state of beatmapsets list to the list
        /// </summary>
        private void SaveBeatmaps()
        {
            // Prepare list of content to save
            var fileContent = new List<string>();

            // For each beatmap...
            foreach (var beatmap in Beatmapsets)
            {
                // Save both urls
                fileContent.Add(beatmap.DiscussionUrl);
                fileContent.Add(@"https://osu.ppy.sh/b/" + beatmap.BeatmapID);
            }

            // Save it to .txt file
            File.WriteAllLines("beatmaps.txt", fileContent);
        }

        #endregion
    }
}
