using System.Windows.Input;
using ReactiveUI;
using SyncedLyricsCreator.Events;
using SyncedLyricsCreator.Helpers.Dialog;
using KeyGesture = Avalonia.Input.KeyGesture;

namespace SyncedLyricsCreator.ViewModels.Dialogs;

/// <summary>
///  The view model for the settings view
/// </summary>
public class SettingsDialogViewModel : ViewModelBase, IModalDialogViewModel
{
    private bool advanceLineAfterSyncing;
    private KeyGesture decreaseTimestampGesture;
    private bool? dialogResult;
    private KeyGesture increaseTimestampGesture;
    private bool isComplete;
    private KeyGesture jumpToTimestampGesture;
    private bool roundTimestampMsToHundredths;
    private KeyGesture setTimestampGesture;
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
    /// Gets or sets a value indicating whether the editor should automatically advance
    /// to the next line after syncing the current one
    /// </summary>
    public bool AdvanceLineAfterSyncing
    {
        get => advanceLineAfterSyncing;
        set => this.RaiseAndSetIfChanged(ref advanceLineAfterSyncing, value);
    }

    /// <summary>
    /// Gets or sets the gesture for the hotkey used to decrease a line's timestamp by 50 ms
    /// </summary>
    public KeyGesture DecreaseTimestampGesture
    {
        get => decreaseTimestampGesture;
        set => this.RaiseAndSetIfChanged(ref decreaseTimestampGesture, value);
    }

    /// <summary>
    /// Gets or sets the gesture for the hotkey used to increase a line's timestamp by 50 ms
    /// </summary>
    public KeyGesture IncreaseTimestampGesture
    {
        get => increaseTimestampGesture;
        set => this.RaiseAndSetIfChanged(ref increaseTimestampGesture, value);
    }

    /// <summary>
    /// Gets or sets the gesture for the hotkey used to jump to a line's timestamp in the player
    /// </summary>
    public KeyGesture JumpToTimestampGesture
    {
        get => jumpToTimestampGesture;
        set => this.RaiseAndSetIfChanged(ref jumpToTimestampGesture, value);
    }

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
    /// Gets or sets the gesture for the hotkey used to set a line's timestamp
    /// </summary>
    public KeyGesture SetTimestampGesture
    {
        get => setTimestampGesture;
        set => this.RaiseAndSetIfChanged(ref setTimestampGesture, value);
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
        AdvanceLineAfterSyncing = Settings.Instance.AdvanceLineAfterSyncing;

        DecreaseTimestampGesture = Settings.Instance.DecreaseTimestampKeyBinding;
        IncreaseTimestampGesture = Settings.Instance.IncreaseTimestampKeyBinding;
        JumpToTimestampGesture = Settings.Instance.JumpToTimestampKeyBinding;
        SetTimestampGesture = Settings.Instance.SetTimestampKeyBinding;

        RoundTimestampMsToHundredths = Settings.Instance.RoundTimestampMsToHundredths;
        TimestampDelayMs = Settings.Instance.TimestampDelayMs;
    }

    private void SaveSettings()
    {
        Settings.Instance.AdvanceLineAfterSyncing = AdvanceLineAfterSyncing;

        Settings.Instance.DecreaseTimestampKeyBinding = DecreaseTimestampGesture;
        Settings.Instance.IncreaseTimestampKeyBinding = IncreaseTimestampGesture;
        Settings.Instance.JumpToTimestampKeyBinding = JumpToTimestampGesture;
        Settings.Instance.SetTimestampKeyBinding = SetTimestampGesture;

        Settings.Instance.RoundTimestampMsToHundredths = RoundTimestampMsToHundredths;
        Settings.Instance.TimestampDelayMs = TimestampDelayMs;

        Settings.Instance.Write();
        MessageBus.Current.SendMessage(new SettingsChangedEventArgs());

        DialogResult = true;
        IsComplete = true;
    }
}
