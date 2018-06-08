using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace GDManager.Core
{
    /// <summary>
    /// Manages the beatmaps in this application
    /// </summary>
    public class BeatmapManager
    {
        #region Public Properties

        /// <summary>
        /// The list which contains every listed beatmap
        /// </summary>
        public ObservableCollection<BeatmapListItemViewModel> Beatmaps { get; set; } = new ObservableCollection<BeatmapListItemViewModel>();

        #endregion

        #region Beatmap Operations

        /// <summary>
        /// Adds specified beatmap to the list
        /// As well as saving it into file
        /// </summary>
        /// <param name="beatmap">The beatmap to add</param>
        public void AddBeatmap(BeatmapListItemViewModel beatmap)
        {
            // Add it to the list (Make sure we do it on UI thread)
            IoC.UI.DispatcherThreadAction(() => Beatmaps.Add(beatmap));

            // Save it to file
            SaveBeatmaps();
        }

        /// <summary>
        /// Deletes specified beatmap from the list
        /// As well as deletes it from file
        /// </summary>
        /// <param name="beatmap">The beatmap to delete</param>
        public void DeleteBeatmap(BeatmapListItemViewModel beatmap)
        {
            // Delete it from the list (Make sure we do it on UI thread)
            IoC.UI.DispatcherThreadAction(() => Beatmaps.Remove(beatmap));

            // Save it to file
            SaveBeatmaps();
        }

        /// <summary>
        /// Loads every saved beatmap to the list
        /// </summary>
        public void LoadBeatmaps()
        {
            // Initialize empty list
            Beatmaps = new ObservableCollection<BeatmapListItemViewModel>();

            // Load everything from file
            var fileContent = File.ReadAllLines("beatmaps.txt");

            // Convert file content into beatmaps
            for (int i = 0; i < fileContent.Length; i++)
            {
                // Get url line
                var difficultyUrl = fileContent[i];

                // Convert it to beatmap
                var beatmap = BeatmapWebHelpers.GetBeatmapFromUrl(difficultyUrl);

                // Add it to the main list
                IoC.UI.DispatcherThreadAction(() => Beatmaps.Add(beatmap));
            }
        }

        /// <summary>
        /// Saves current beatmaps list state to file
        /// </summary>
        public void SaveBeatmaps()
        {
            // Prepare list of content to save
            var fileContent = new List<string>();

            // For each beatmap...
            foreach (var beatmap in Beatmaps)
            {
                // Save beatmap url
                var slashAppender = beatmap.DiscussionUrl.EndsWith("/") ? "" : "/";
                fileContent.Add(beatmap.DiscussionUrl + slashAppender + beatmap.BeatmapID);
            }

            // Save it to .txt file
            File.WriteAllLines("beatmaps.txt", fileContent);
        }

        #endregion
    }
}
