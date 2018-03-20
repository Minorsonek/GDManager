using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace GDManager.Core
{
    /// <summary>
    /// The view model for main beatmap list
    /// </summary>
    public class BeatmapListViewModel : BaseViewModel
    {
        #region Singleton

        /// <summary>
        /// Create only one instance of this view model
        /// </summary>
        public static BeatmapListViewModel Instance { get; set; } = new BeatmapListViewModel();

        #endregion

        #region Public Properties

        /// <summary>
        /// Main list containing every listed beatmap
        /// </summary>
        public ObservableCollection<BeatmapListItemViewModel> Beatmaps { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeatmapListViewModel()
        {

        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Adds specified beatmap to the list
        /// As well as saving it into file
        /// </summary>
        /// <param name="beatmap">The beatmap to add</param>
        public void AddBeatmap(BeatmapListItemViewModel beatmap)
        {
            // Add it to the list
            Beatmaps.Add(beatmap);

            // Save it to file
        }

        /// <summary>
        /// Deletes specified beatmap from the list
        /// As well as deletes it from file
        /// </summary>
        /// <param name="beatmap">The beatmap to delete</param>
        public void DeleteBeatmap(BeatmapListItemViewModel beatmap)
        {
            // Delete it from the list
            Beatmaps.Remove(beatmap);

            // Delete it from file
            var content = File.ReadAllLines("beatmaps.txt");
            var newContent = new List<string>();
            foreach (var line in content)
                if (!line.StartsWith(beatmap.DiscussionUrl))
                    newContent.Add(line);

            File.WriteAllLines("beatmaps.txt", newContent.ToArray());
        }

        #endregion
    }
}
