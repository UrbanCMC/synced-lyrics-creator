using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SyncedLyricsCreator.Views
{
    /// <summary>
    /// Interaction logic for the player view
    /// </summary>
    public partial class PlayerView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerView"/> class.
        /// </summary>
        public PlayerView() => InitializeComponent();

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
