namespace SyncedLyricsCreator.Helpers.Dialog.MessageBox
{
    /// <summary>
    /// An enumeration of results returned from a message box
    /// </summary>
    internal enum MessageBoxResult
    {
        /// <summary>
        /// The message box returns no result.
        /// </summary>
        None = 0,

        /// <summary>
        /// The result value of the message box is OK.
        /// </summary>
        OK = 1,

        /// <summary>
        /// The result value of the message box is Cancel.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The result value of the message box is Yes.
        /// </summary>
        Yes = 6,

        /// <summary>
        /// The result value of the message box is No.
        /// </summary>
        No = 7,
    }
}
