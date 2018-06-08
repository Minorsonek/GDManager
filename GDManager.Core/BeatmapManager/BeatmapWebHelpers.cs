using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace GDManager.Core
{
    /// <summary>
    /// Helpers for handling web communication to receive beatmaps
    /// </summary>
    public static class BeatmapWebHelpers
    {
        #region Public Methods

        /// <summary>
        /// Gets <see cref="BeatmapListItemViewModel"/> from specified url
        /// </summary>
        public static BeatmapListItemViewModel GetBeatmapFromUrl(string difficultyUrl)
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
        /// Checks every listed beatmap for mods
        /// </summary>
        public static void CheckMods()
        {
            // Check each beatmapset...
            foreach (var beatmapset in IoC.BeatmapManager.Beatmaps)
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
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Converts the difficulty specific url to the beatmapset discussion thread url
        /// </summary>
        private static string ConvertDiffUrlThread(string difficultyUrl) => difficultyUrl.Substring(0, difficultyUrl.IndexOf("discussion") + "discussion".Length);

        /// <summary>
        /// Returns the <see cref="BeatmapsetDiscussion"/> object from the osu! website
        /// </summary>
        private static BeatmapsetDiscussion GetBeatmapsetDiscussionByURL(string url)
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
        private static string GetBeatmapIDFromURL(string url)
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
        private static HtmlNode GetJsonScriptTag(IEnumerable<HtmlNode> scriptNodes)
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
