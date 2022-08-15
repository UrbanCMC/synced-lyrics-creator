using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using SyncedLyricsCreator.Events;

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
    }
}
