using System.Collections.ObjectModel;

namespace GDManager.Core
{
    /// <summary>
    /// The extension methods for main beatmap list
    /// </summary>
    public static class BeatmapListExtensions
    {
        /// <summary>
        /// Adds specified beatmap to the list
        /// As well as saving it into file
        /// </summary>
        /// <param name="beatmap">The beatmap to add</param>
        public static void AddBeatmap(this ObservableCollection<BeatmapListItemViewModel> list, BeatmapListItemViewModel beatmap)
        {
            // Add it to the list
            list.Add(beatmap);

            // Save it to file
            IoC.BeatmapManager.SaveBeatmaps();
        }

        /// <summary>
        /// Deletes specified beatmap from the list
        /// As well as deletes it from file
        /// </summary>
        /// <param name="beatmap">The beatmap to delete</param>
        public static void DeleteBeatmap(this ObservableCollection<BeatmapListItemViewModel> list, BeatmapListItemViewModel beatmap)
        {
            // Delete it from the list
            list.Remove(beatmap);

            // Save it to file
            IoC.BeatmapManager.SaveBeatmaps();
        }
    }
}
