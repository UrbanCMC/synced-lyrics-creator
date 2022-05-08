using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SyncedLyricsCreator.Views
{
    /// <summary>
    /// Interaction logic for LyricsEditorView.xaml
    /// </summary>
    public partial class LyricsEditorView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsEditorView"/> class.
        /// </summary>
        public LyricsEditorView() => InitializeComponent();

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
