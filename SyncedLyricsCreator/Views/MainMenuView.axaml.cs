using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SyncedLyricsCreator.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.axaml
    /// </summary>
    public partial class MainMenuView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuView"/> class.
        /// </summary>
        public MainMenuView() => InitializeComponent();

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
