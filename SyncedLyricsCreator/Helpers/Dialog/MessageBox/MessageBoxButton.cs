namespace SyncedLyricsCreator.Helpers.Dialog.MessageBox
{
    /// <summary>
    /// Specifies an enumeration of button combinations that can be displayed on a message box
    /// </summary>
    internal enum MessageBoxButton
    {
        /// <summary>
        /// The message box displays an OK button.
        /// </summary>
        OK = 0,

        /// <summary>
        /// The message box displays OK and Cancel buttons.
        /// </summary>
        OKCancel = 1,

        /// <summary>
        /// The message box displays Yes, No, and Cancel buttons.
        /// </summary>
        YesNoCancel = 2,

        /// <summary>
        /// The message box displays Yes and No buttons.
        /// </summary>
        YesNo = 4,
    }
}
