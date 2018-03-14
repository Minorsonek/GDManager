namespace GDManager.Core
{
    /// <summary>
    /// The osu! beatmap object
    /// </summary>
    public class Beatmap
    {
        #region Public Properties

        /// <summary>
        /// The id of the beatmap
        /// </summary>
        public string BeatmapID { get; set; }

        /// <summary>
        /// The title (song name, artist)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The original url beatmap was added by
        /// </summary>
        public string BeatmapURL { get; set; }

        /// <summary>
        /// Indicates if there is new mod to be answered on the beatmap's discussion
        /// </summary>
        public bool IsNewMod { get; set; }

        #endregion
    }
}
