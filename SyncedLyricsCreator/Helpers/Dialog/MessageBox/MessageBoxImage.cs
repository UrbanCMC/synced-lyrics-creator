namespace SyncedLyricsCreator.Helpers.Dialog.MessageBox
{
    /// <summary>
    /// Specifies an enumeration of images that can be displayed in a message box
    /// </summary>
    internal enum MessageBoxImage
    {
        /// <summary>
        /// The message box contains no symbols.
        /// </summary>
        None = 0,

        /// <summary>
        /// The message box contains a symbol consisting of white X in a circle with a red background.
        /// </summary>
        Error = 1,

        /// <summary>
        /// The message box contains a symbol consisting of a question mark in a circle.
        /// </summary>
        Question = 2,

        /// <summary>
        /// The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.
        /// </summary>
        Warning = 4,

        /// <summary>
        /// The message box contains a symbol consisting of a lowercase letter i in a circle.
        /// </summary>
        Information = 8,
    }
}
