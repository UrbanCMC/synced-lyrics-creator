using ReactiveUI;

namespace SyncedLyricsCreator.ViewModels
{
    /// <summary>
    /// The view model for the main window
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private LyricsEditorViewModel lyricsEditorViewModel;
        private PlayerViewModel playerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainWindowViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            LyricsEditorViewModel = new LyricsEditorViewModel();
            PlayerViewModel = new PlayerViewModel();
        }

        /// <summary>
        /// Gets or sets the view model for the lyrics editor
        /// </summary>
        public LyricsEditorViewModel LyricsEditorViewModel
        {
            get => lyricsEditorViewModel;
            set => this.RaiseAndSetIfChanged(ref lyricsEditorViewModel, value);
        }

        /// <summary>
        /// Gets or sets the view model for the media player
        /// </summary>
        public PlayerViewModel PlayerViewModel
        {
            get => playerViewModel;
            set => this.RaiseAndSetIfChanged(ref playerViewModel, value);
        }
    }
}
