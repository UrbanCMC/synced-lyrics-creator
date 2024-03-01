using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using SyncedLyricsCreator.ViewModels.Dialogs;

namespace SyncedLyricsCreator.Views.Dialogs;

/// <summary>
/// Interaction logic for LYricsSearchView.axaml
/// </summary>
public partial class LyricsSearchView : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LyricsSearchView"/> class
    /// </summary>
    public LyricsSearchView() => InitializeComponent();

    /// <summary>
    /// Handles the Click event for a button closing the dialog
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="e">The event args</param>
    public void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        (DataContext as LyricsSearchViewModel)!.SelectResult();
    }
}
