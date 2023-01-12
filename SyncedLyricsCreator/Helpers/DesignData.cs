using SyncedLyricsCreator.ViewModels;
using SyncedLyricsCreator.ViewModels.Dialogs;

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
        /// Gets a design-time <see cref="MainMenuViewModel"/>
        /// </summary>
        public static MainMenuViewModel MainMenuViewModel { get; } = new();

        /// <summary>
        /// Gets a design-time <see cref="PlayerViewModel"/>
        /// </summary>
        public static PlayerViewModel PlayerViewModel { get; } = new();

        /// <summary>
        /// Gets a design-time <see cref="MainWindowViewModel"/>
        /// </summary>
        public static MainWindowViewModel MainWindowViewModel { get; } = new(LyricsEditorViewModel, MainMenuViewModel, PlayerViewModel);

        /// <summary>
        /// Gets a design-time <see cref="SettingsDialogViewModel"/>
        /// </summary>
        public static SettingsDialogViewModel SettingsDialogViewModel { get; } = new() { RoundTimestampMsToHundredths = true, TimestampDelayMs = 300 };
    }
}
