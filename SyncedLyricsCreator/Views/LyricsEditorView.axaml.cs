using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SyncedLyricsCreator.ViewModels;

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
        public LyricsEditorView()
        {
            InitializeComponent();
            DataContextChanged += (_, _) => RegisterHotkeys();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void RegisterHotkeys()
        {
            var lyricsTextBox = this.FindControl<TextBox>("LyricsTextBox");
            if (DataContext is not LyricsEditorViewModel vm || lyricsTextBox == null)
            {
                return;
            }

            lyricsTextBox.KeyBindings.Clear();
            lyricsTextBox.KeyBindings.Add(new KeyBinding { Gesture = Settings.Instance.DecreaseTimestampKeyBinding, Command = vm.DecreaseTimestampCommand });
            lyricsTextBox.KeyBindings.Add(new KeyBinding { Gesture = Settings.Instance.IncreaseTimestampKeyBinding, Command = vm.IncreaseTimestampCommand });
            lyricsTextBox.KeyBindings.Add(new KeyBinding { Gesture = Settings.Instance.JumpToTimestampKeyBinding, Command = vm.JumpToTimestampCommand });
            lyricsTextBox.KeyBindings.Add(new KeyBinding { Gesture = Settings.Instance.SetTimestampKeyBinding, Command = vm.SetTimestampCommand });
        }
    }
}
