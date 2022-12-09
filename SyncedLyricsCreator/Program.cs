using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Splat;

namespace SyncedLyricsCreator
{
    /// <summary>
    /// The entry class of this application
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Avalonia configuration, don't remove; also used by visual designer.
        /// </summary>
        /// <returns>The configured <see cref="AppBuilder"/></returns>
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        /// <summary>
        /// Initialization code. Don't use any Avalonia, third-party APIs or any
        /// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        /// yet and stuff might break.
        /// </summary>
        /// <param name="args">An array of arguments passed to the application</param>
        [STAThread]
        public static void Main(string[] args)
        {
            Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

            LoadSettings();

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        private static void LoadSettings()
        {
            // Call both read and write to ensure any new settings are added to the file
            Settings.Instance.Read();
            Settings.Instance.Write();
        }
    }
}
