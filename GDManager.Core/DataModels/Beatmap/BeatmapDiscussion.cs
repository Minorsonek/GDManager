using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GDManager.Core
{
    /// <summary>
    /// Contains informations about the whole beatmapset's discussion thread
    /// </summary>
    public class BeatmapsetDiscussion
    {
        #region Public Properties

        /// <summary>
        /// The ID of this beatmapset
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The title of the song
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The artist who made the song
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The url for thumbnail of this beatmapset
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The listed tags under the beatmapset
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Indicates if the beatmapset is already ranked
        /// </summary>
        public bool Ranked { get; set; }

        /// <summary>
        /// Indicates if further discussion in this thread is available
        /// </summary>
        public bool DiscussionEnabled { get; set; }

        /// <summary>
        /// The list of every difficulty in that beatmapset
        /// </summary>
        public List<Beatmap> Beatmaps { get; set; }

        /// <summary>
        /// The list of every discussion posted in the thread
        /// </summary>
        public List<Discussion> Discussions { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeatmapsetDiscussion(JObject jsonCode)
        {
            // Get the beatmapset json tag
            var beatmapsetJson = jsonCode["beatmapset"];

            // Match every property with that came from json
            ID = (string)beatmapsetJson["id"];
            Title = (string)beatmapsetJson["title"];
            Artist = (string)beatmapsetJson["artist"];
            Tags = beatmapsetJson["tags"].ToString().Split(' ');
            Ranked = (bool)beatmapsetJson["ranked"];
            DiscussionEnabled = (bool)beatmapsetJson["discussion_enabled"];
            ImageUrl = (string)beatmapsetJson["covers"]["list@2x"];

            Beatmaps = new List<Beatmap>();
            foreach (var beatmapJson in beatmapsetJson["beatmaps"])
                Beatmaps.Add(new Beatmap(beatmapJson));

            Discussions = new List<Discussion>();
            foreach (var discussionJson in beatmapsetJson["discussions"])
                Discussions.Add(new Discussion(discussionJson));
        }

        #endregion
    }
}
