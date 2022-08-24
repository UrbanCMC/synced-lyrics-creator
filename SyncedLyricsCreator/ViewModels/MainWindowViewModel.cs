using System;
using ReactiveUI;
using SyncedLyricsCreator.Data.Models;
using SyncedLyricsCreator.Data.Services;
using SyncedLyricsCreator.Events;
using SyncedLyricsCreator.Helpers.Dialog;
using SyncedLyricsCreator.Helpers.Dialog.MessageBox;

namespace SyncedLyricsCreator.ViewModels
{
    /// <summary>
    /// The view model for the main window
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private LyricsEditorViewModel lyricsEditorViewModel = null!;
        private MainMenuViewModel mainMenuViewModel = null!;
        private PlayerViewModel playerViewModel = null!;

        private string title = "Synced Lyrics Creator";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="lyricsEditorViewModel">The view model for the lyrics editor</param>
        /// <param name="mainMenuViewModel">The view model for the main menu</param>
        /// <param name="playerViewModel">The view model for the player</param>
        public MainWindowViewModel(LyricsEditorViewModel lyricsEditorViewModel, MainMenuViewModel mainMenuViewModel, PlayerViewModel playerViewModel)
        {
            LyricsEditorViewModel = lyricsEditorViewModel;
            MainMenuViewModel = mainMenuViewModel;
            PlayerViewModel = playerViewModel;

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
        /// Gets or sets the view model for the main menu
        /// </summary>
        public MainMenuViewModel MainMenuViewModel
        {
            get => mainMenuViewModel;
            set => this.RaiseAndSetIfChanged(ref mainMenuViewModel, value);
        }

        /// <summary>
        /// Gets or sets the view model for the media player
        /// </summary>
        public PlayerViewModel PlayerViewModel
        {
            get => playerViewModel;
            set => this.RaiseAndSetIfChanged(ref playerViewModel, value);
        }

        /// <summary>
        /// Gets or sets the window title
        /// </summary>
        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        public async void LoadTrack(string path)
        {
            var lyrics = LyricsConverter.LoadFromFile(path);
            if (lyrics == null)
            {
                await DialogService.ShowMessageBox("Error while opening media file.", "Error", icon: MessageBoxImage.Error);
                lyrics = new Lyrics();
            }

            Title = $"Synced Lyrics Creator - {System.IO.Path.GetFileName(path)}";

            LyricsEditorViewModel.Load(lyrics);
            PlayerViewModel.LoadFile(path);
        }

        private void OnLoadTrack(OnLoadTrackEventArgs args) => LoadTrack(args.Path);

        private void OnSaveTrack(OnSaveTrackEventArgs args)
        {
            var lyrics = LyricsEditorViewModel.GetLyrics();
            LyricsConverter.SaveToFile(args.Path, lyrics);
        }
    }
}
