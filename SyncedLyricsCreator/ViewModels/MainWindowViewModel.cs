using System;
using ReactiveUI;
using SyncedLyricsCreator.Data.Models;
using SyncedLyricsCreator.Data.Services;
using SyncedLyricsCreator.Events;

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

            MessageBus.Current.Listen<OnLoadTrackEventArgs>()
                .Subscribe(OnLoadTrack);
            MessageBus.Current.Listen<OnSaveTrackEventArgs>()
                .Subscribe(OnSaveTrack);
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

        private void OnLoadTrack(OnLoadTrackEventArgs args)
        {
            var lyrics = LyricsConverter.LoadFromFile(args.Path);
            if (lyrics == null)
            {
                // TODO: Show message about invalid file
                lyrics = new Lyrics();
            }

            LyricsEditorViewModel.Load(lyrics);
            PlayerViewModel.LoadFile(args.Path);
        }

        private void OnSaveTrack(OnSaveTrackEventArgs args)
        {
            var lyrics = LyricsEditorViewModel.GetLyrics();
            LyricsConverter.SaveToFile(args.Path, lyrics);
        }
    }
}
