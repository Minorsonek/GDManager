﻿using Newtonsoft.Json.Linq;

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
            // Don't save null post, as website may have ones
            if (!postJson.HasValues)
                return;

            // Match every property with that came from json
            ID = (string)postJson["id"];
            DiscussionID = (string)postJson["beatmap_discussion_id"];

            try
            {
                // Try to convert the message
                Message = (string)postJson["message"];
            }
            catch
            {
                // If failed, it means that the message type is resolved and we dont handle that type of messages
                Message = "RESOLVED";
            }
        }

        #endregion
    }
}
