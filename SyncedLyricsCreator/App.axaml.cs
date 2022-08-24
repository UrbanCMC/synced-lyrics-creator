using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using SyncedLyricsCreator.Extensions;
using SyncedLyricsCreator.ViewModels;
using SyncedLyricsCreator.Views;

namespace SyncedLyricsCreator
{
    /// <summary>
    /// Base application class that launches the main window
    /// </summary>
    public partial class App : Application
    {
        /// <inheritdoc/>
        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        /// <inheritdoc/>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var fileArgument = desktop.Args.Length >= 1 ? desktop.Args[0] : null;
                var context = Locator.Current.GetRequiredService<MainWindowViewModel>();
                if (File.Exists(fileArgument))
                {
                    context.LoadTrack(fileArgument!);
                }

                desktop.MainWindow = new MainWindow { DataContext = context };
                desktop.MainWindow.Closing += (sender, args) =>
                {
                    args.Cancel = true;
                    context.OnClosing();
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
