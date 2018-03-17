using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GDManager.Core
{
    /// <summary>
    /// The model of specific discussion in beatmap's discussion thread
    /// </summary>
    public class Discussion
    {
        #region Public Properties

        /// <summary>
        /// The ID of this specific discussion
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The beatmapset ID this discussion belongs to
        /// </summary>
        public string BeatmapsetID { get; set; }

        /// <summary>
        /// The beatmap ID under which this discussion has been written
        /// </summary>
        public string BeatmapID { get; set; }

        /// <summary>
        /// Indicates if this discussion is already resolved by a mapper
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// Indicates if this discussion is resolvable (Praise/Hype comments can't be resolved)
        /// </summary>
        public bool CanBeResolved { get; set; }

        /// <summary>
        /// The list of posts in the discussion
        /// </summary>
        public List<DiscussionPost> Posts { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Discussion(JToken discussionJson)
        {
            // Don't save null discussion, as website may have ones
            if (!discussionJson.HasValues)
                return;

            // Match every property with that came from json
            ID = (string)discussionJson["id"];
            BeatmapsetID = (string)discussionJson["beatmapset_id"];
            BeatmapID = (string)discussionJson["beatmap_id"];
            IsResolved = (bool)discussionJson["resolved"];
            CanBeResolved = (bool)discussionJson["can_be_resolved"];

            Posts = new List<DiscussionPost>();
            foreach (var postJson in discussionJson["posts"])
                Posts.Add(new DiscussionPost(postJson));
        }

        #endregion
    }
}
