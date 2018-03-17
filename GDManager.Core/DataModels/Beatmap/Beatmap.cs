using Newtonsoft.Json.Linq;

namespace GDManager.Core
{
    /// <summary>
    /// The osu! beatmap object
    /// </summary>
    public class Beatmap
    {
        #region Public Properties

        /// <summary>
        /// The ID of this specific beatmap
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The beatmapset ID this diff belongs to
        /// </summary>
        public string BeatmapsetID { get; set; }

        /// <summary>
        /// The name of this beatmap
        /// </summary>
        public string DiffName { get; set; }

        /// <summary>
        /// The star rating of this beatmap
        /// </summary>
        public string DiffStarRating { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Beatmap(JToken beatmapJson)
        {
            // Don't save null beatmap, as website may have ones
            if (!beatmapJson.HasValues)
                return;
            
            // Match every property with that came from json
            ID = (string)beatmapJson["id"];
            BeatmapsetID = (string)beatmapJson["beatmapset_id"];
            DiffName = (string)beatmapJson["version"];
            DiffStarRating = (string)beatmapJson["difficulty_rating"];
        }

        #endregion
    }
}
