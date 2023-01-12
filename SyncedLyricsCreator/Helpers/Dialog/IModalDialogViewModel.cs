namespace SyncedLyricsCreator.Helpers.Dialog;

/// <summary>
/// An interface describing a modal dialog returning a dialog result
/// </summary>
internal interface IModalDialogViewModel
{
    /// <summary>
    /// Gets the result of the dialog
    /// </summary>
    bool? DialogResult { get; }

    /// <summary>
    /// Gets a value indicating whether the dialog has been completed and should close
    /// </summary>
    bool IsComplete { get; }
}
