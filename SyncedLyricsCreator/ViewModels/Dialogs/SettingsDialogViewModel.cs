using System.Windows.Input;
using ReactiveUI;
using SyncedLyricsCreator.Helpers.Dialog;

namespace SyncedLyricsCreator.ViewModels.Dialogs;

/// <summary>
///  The view model for the settings view
/// </summary>
public class SettingsDialogViewModel : ViewModelBase, IModalDialogViewModel
{
    private bool? dialogResult;
    private bool isComplete;
    private bool roundTimestampMsToHundredths;
    private int timestampDelayMs;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsDialogViewModel"/> class.
    /// </summary>
    public SettingsDialogViewModel()
    {
        // Set-up commands
        OkCommand = ReactiveCommand.Create(SaveSettings);

        LoadSettings();
    }

    /// <inheritdoc />
    public bool? DialogResult
    {
        get => dialogResult;
        private set => this.RaiseAndSetIfChanged(ref dialogResult, value);
    }

    /// <inheritdoc />
    public bool IsComplete
    {
        get => isComplete;
        private set => this.RaiseAndSetIfChanged(ref isComplete, value);
    }

    /// <summary>
    /// Gets the command for the OK button
    /// </summary>
    public ICommand OkCommand { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the milliseconds of timestamps should be
    /// rounded to the hundredths position (e.g. <c>[00:01.123]</c> -> <c>[00:01.120]</c>)
    /// </summary>
    public bool RoundTimestampMsToHundredths
    {
        get => roundTimestampMsToHundredths;
        set => this.RaiseAndSetIfChanged(ref roundTimestampMsToHundredths, value);
    }

    /// <summary>
    /// Gets or sets the number of milliseconds to subtract from the
    /// actual timestamp when syncing a line to make it easier to get the timing right
    /// </summary>
    public int TimestampDelayMs
    {
        get => timestampDelayMs;
        set => this.RaiseAndSetIfChanged(ref timestampDelayMs, value);
    }

    private void LoadSettings()
    {
        RoundTimestampMsToHundredths = Settings.Instance.RoundTimestampMsToHundredths;
        TimestampDelayMs = Settings.Instance.TimestampDelayMs;
    }

    private void SaveSettings()
    {
        Settings.Instance.RoundTimestampMsToHundredths = RoundTimestampMsToHundredths;
        Settings.Instance.TimestampDelayMs = TimestampDelayMs;

        Settings.Instance.Write();

        DialogResult = true;
        IsComplete = true;
    }
}
