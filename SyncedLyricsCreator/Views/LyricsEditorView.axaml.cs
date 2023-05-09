using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using ReactiveUI;
using SyncedLyricsCreator.Events;
using SyncedLyricsCreator.ViewModels;
using RegistryOptions = SyncedLyricsCreator.Grammars.RegistryOptions;

namespace SyncedLyricsCreator.Views
{
    /// <summary>
    /// Interaction logic for LyricsEditorView.xaml
    /// </summary>
    public partial class LyricsEditorView : UserControl
    {
        private const string TextMateGrammar = "source.syncedLyrics";

        private readonly TextEditor editor;
        private LyricsEditorViewModel lyricsEditorVm = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsEditorView"/> class.
        /// </summary>
        public LyricsEditorView()
        {
            InitializeComponent();
            DataContextChanged += (_, _) =>
            {
                lyricsEditorVm = (LyricsEditorViewModel)DataContext!;
                RegisterHotkeys();
            };
            MessageBus.Current.Listen<SettingsChangedEventArgs>()
                .Subscribe(new Action<object>(_ => RegisterHotkeys()));

            editor = this.FindControl<TextEditor>("LyricsTextBox")!;
            var registryOptions = new RegistryOptions();
            var textMateInstallation = editor.InstallTextMate(registryOptions);

            textMateInstallation.SetGrammar(TextMateGrammar);
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void RegisterHotkeys()
        {
            var commandBindings = new[]
            {
                new RoutedCommandBinding(
                    new RoutedCommand(nameof(lyricsEditorVm.DecreaseTimestampCommand), Settings.Instance.DecreaseTimestampKeyBinding)
                    , (_, _) => lyricsEditorVm.DecreaseTimestampCommand.Execute(null))
                , new RoutedCommandBinding(
                    new RoutedCommand(nameof(lyricsEditorVm.IncreaseTimestampCommand), Settings.Instance.IncreaseTimestampKeyBinding)
                    , (_, _) => lyricsEditorVm.IncreaseTimestampCommand.Execute(null))
                , new RoutedCommandBinding(
                    new RoutedCommand(nameof(lyricsEditorVm.JumpToTimestampCommand), Settings.Instance.JumpToTimestampKeyBinding)
                    , (_, _) => lyricsEditorVm.JumpToTimestampCommand.Execute(null))
                , new RoutedCommandBinding(
                    new RoutedCommand(nameof(lyricsEditorVm.SetTimestampCommand), Settings.Instance.SetTimestampKeyBinding)
                    , (_, _) => lyricsEditorVm.SetTimestampCommand.Execute(null))
            };

            foreach (var binding in commandBindings)
            {
                editor.TextArea.DefaultInputHandler.CommandBindings.Remove(
                    editor.TextArea.CommandBindings.FirstOrDefault(x => x.Command.Name == binding.Command.Name));
                editor.TextArea.DefaultInputHandler.CommandBindings.Add(binding);
            }
        }
    }
}
