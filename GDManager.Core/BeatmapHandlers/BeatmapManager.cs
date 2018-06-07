using System.Collections.Generic;
using System.IO;

namespace GDManager.Core
{
    /// <summary>
    /// Manages the beatmaps in this application
    /// </summary>
    public class BeatmapManager
    {
        /// <summary>
        /// Saves current beatmaps list state to file
        /// </summary>
        public void SaveBeatmaps()
        {
            // Prepare list of content to save
            var fileContent = new List<string>();

            // For each beatmap...
            foreach (var beatmap in IoC.Beatmaps)
            {
                // Save beatmap url
                var slashAppender = beatmap.DiscussionUrl.EndsWith("/") ? "" : "/";
                fileContent.Add(beatmap.DiscussionUrl + slashAppender + beatmap.BeatmapID);
            }

            // Save it to .txt file
            File.WriteAllLines("beatmaps.txt", fileContent);
        }
    }
}
