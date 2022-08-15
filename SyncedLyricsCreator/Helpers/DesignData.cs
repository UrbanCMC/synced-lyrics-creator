using SyncedLyricsCreator.ViewModels;

namespace SyncedLyricsCreator.Helpers
{
    /// <summary>
    /// Provides design-time data to make the visual design tools more useful
    /// </summary>
    public static class DesignData
    {
        /// <summary>
        /// Gets a design-time <see cref="LyricsEditorViewModel"/>
        /// </summary>
        public static LyricsEditorViewModel LyricsEditorViewModel { get; } = new();

        /// <summary>
        /// Gets a design-time <see cref="PlayerViewModel"/>
        /// </summary>
        public static PlayerViewModel PlayerViewModel { get; } = new();

        /// <summary>
        /// Gets a design-time <see cref="MainWindowViewModel"/>
        /// </summary>
        public static MainWindowViewModel MainWindowViewModel { get; } = new()
        {
            LyricsEditorViewModel = LyricsEditorViewModel,
            PlayerViewModel = PlayerViewModel,
        };
    }
}
