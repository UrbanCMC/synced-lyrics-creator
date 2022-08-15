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
                desktop.MainWindow = new MainWindow { DataContext = Locator.Current.GetRequiredService<MainWindowViewModel>() };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
