using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Accessibility;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using SyncedLyricsCreator.Events;
using SyncedLyricsCreator.Helpers.Dialog;
using SyncedLyricsCreator.ViewModels.Dialogs;

namespace SyncedLyricsCreator.ViewModels
{
    /// <summary>
    /// View model implementation for the main menu
    /// </summary>
    public class MainMenuViewModel : ViewModelBase
    {
        private string currentFilePath = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuViewModel"/> class
        /// </summary>
        public MainMenuViewModel()
        {
            ExitCommand = ReactiveCommand.Create(Exit);
            OpenCommand = ReactiveCommand.Create(PublishLoadEventAsync);
            SaveCommand = ReactiveCommand.Create(PublishSaveEvent);
            SearchCommand = ReactiveCommand.CreateFromTask(ShowSearch);
            SettingsCommand = ReactiveCommand.Create(ShowSettings);
        }

        /// <summary>
        /// Gets the command to exit the application
        /// </summary>
        public ICommand ExitCommand { get; }

        /// <summary>
        /// Gets the command to open a new file
        /// </summary>
        public ICommand OpenCommand { get; }

        /// <summary>
        /// Gets the command to save the lyrics currently in the editor
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Gets the command to search for lyrics on the web
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        /// Gets the command to show the settings window
        /// </summary>
        public ICommand SettingsCommand { get; }

        private void Exit()
        {
            // TODO: Handle unsaved changes
            Environment.Exit(0);
        }

        private async Task PublishLoadEventAsync()
        {
            var ofd = new OpenFileDialog()
            {
                AllowMultiple = false
                , Title = "Select file to open..."
                , Filters = new List<FileDialogFilter>
                {
                    new() { Name = "Audio files", Extensions = new List<string> { "mp3", "flac", "wav" } }
                    , new() { Name = "All files", Extensions = new List<string> { "*" } }
                }
            };

            var result = await ofd.ShowAsync((Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow)
                ?? Array.Empty<string>();
            if (result.Length < 1)
            {
                return;
            }

            currentFilePath = result[0];

            MessageBus.Current.SendMessage(new OnLoadTrackEventArgs(currentFilePath));
        }

        private void PublishSaveEvent()
        {
            if (string.IsNullOrWhiteSpace(currentFilePath))
            {
                return;
            }

            MessageBus.Current.SendMessage(new OnSaveTrackEventArgs(currentFilePath));
        }

        private async Task ShowSearch()
        {
            var vm = new LyricsSearchViewModel(currentFilePath);
            var selectedLyrics = await DialogService.ShowDialog(vm);
            if (selectedLyrics == true)
            {
                var lyrics = vm.SelectedSearchResult!.SyncedLyrics;
                if (string.IsNullOrWhiteSpace(lyrics))
                {
                    lyrics = vm.SelectedSearchResult!.PlainLyrics;
                }

                MessageBus.Current.SendMessage(new OnSelectLyricsEventArgs(lyrics));
            }
        }

        private void ShowSettings() => _ = DialogService.ShowDialog(new SettingsDialogViewModel());
    }
}
