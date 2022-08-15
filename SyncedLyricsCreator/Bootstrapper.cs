using Splat;
using SyncedLyricsCreator.Extensions;
using SyncedLyricsCreator.ViewModels;

namespace SyncedLyricsCreator
{
    /// <summary>
    /// Registers DI services required for the application to work
    /// </summary>
    internal static class Bootstrapper
    {
        /// <summary>
        /// Registers DI services required for the application to work
        /// </summary>
        /// <param name="services">The resolver used for registering services</param>
        /// <param name="resolver">The resolver used for constructor arguments of registered services</param>
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            // ViewModels
            services.Register(() => new LyricsEditorViewModel());
            services.Register(() => new MainMenuViewModel());
            services.Register(() => new PlayerViewModel());
            services.Register(
                () => new MainWindowViewModel(
                    resolver.GetRequiredService<LyricsEditorViewModel>()
                    , resolver.GetRequiredService<MainMenuViewModel>()
                    , resolver.GetRequiredService<PlayerViewModel>()));
        }
    }
}
