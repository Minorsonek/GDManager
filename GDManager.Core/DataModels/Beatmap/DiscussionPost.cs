using Newtonsoft.Json.Linq;

namespace GDManager.Core
{
    /// <summary>
    /// The user post in discussion with his mod under a beatmap
    /// </summary>
    public class DiscussionPost
    {
        #region Public Properties

        /// <summary>
        /// The ID of this post
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The discussion ID this post belongs to
        /// </summary>
        public string DiscussionID { get; set; }

        /// <summary>
        /// The message of this post containg the user's mod
        /// </summary>
        public string Message { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscussionPost(JToken postJson)
        {
            // Match every property with that came from json
            ID = (string)postJson["id"];
            DiscussionID = (string)postJson["beatmap_discussion_id"];
            Message = (string)postJson["message"];
        }

        #endregion
    }
}
