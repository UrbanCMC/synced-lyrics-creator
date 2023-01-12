using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SyncedLyricsCreator.Views.Dialogs;

/// <summary>
/// Interaction logic for SettingsDialogView.axaml
/// </summary>
public partial class SettingsDialogView : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsDialogView"/> class.
    /// </summary>
    public SettingsDialogView() => InitializeComponent();

    /// <summary>
    /// Handles the Click event for a button closing the dialog
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="e">The event args</param>
    public void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
