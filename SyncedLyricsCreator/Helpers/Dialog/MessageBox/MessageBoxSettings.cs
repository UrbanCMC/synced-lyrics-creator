namespace SyncedLyricsCreator.Helpers.Dialog.MessageBox
{
    /// <summary>
    /// Defines the settings applied to a message box
    /// </summary>
    internal class MessageBoxSettings
    {
        private readonly string buttonCancelText = "Cancel";
        private readonly string buttonNoText = "No";
        private readonly string buttonOkText = "OK";
        private readonly string buttonYesText = "Yes";
        private readonly string caption = "MessageBox";

        /// <summary>
        /// Gets the <see cref="MessageBoxButton"/> value that specifies which button or buttons to display.
        /// Default value is <see cref="MessageBoxButton.OK"/>.
        /// </summary>
        public MessageBoxButton Button { get; init; }

        /// <summary>
        /// Gets the text for the Cancel button. If not set, the default text will be used.
        /// </summary>
        public string ButtonCancelText
        {
            get => buttonCancelText;
            init => SetIfNotNullOrWhiteSpace(ref buttonCancelText, value);
        }

        /// <summary>
        /// Gets the text for the No button. If not set, the default text will be used.
        /// </summary>
        public string ButtonNoText
        {
            get => buttonNoText;
            init => SetIfNotNullOrWhiteSpace(ref buttonNoText, value);
        }

        /// <summary>
        /// Gets the text for the Ok button. If not set, the default text will be used.
        /// </summary>
        public string ButtonOkText
        {
            get => buttonOkText;
            init => SetIfNotNullOrWhiteSpace(ref buttonOkText, value);
        }

        /// <summary>
        /// Gets the text for the Yes button. If not set, the default text will be used.
        /// </summary>
        public string ButtonYesText
        {
            get => buttonYesText;
            init => SetIfNotNullOrWhiteSpace(ref buttonYesText, value);
        }

        /// <summary>
        /// Gets the string that specifies the title bar caption to display.
        /// Default value is an empty string.
        /// </summary>
        public string Caption
        {
            get => caption;
            init => SetIfNotNullOrWhiteSpace(ref caption, value);
        }

        /// <summary>
        /// Gets the <see cref="MessageBoxResult"/> value that specifies the default result of the message box.
        /// Default value is <see cref="MessageBoxResult.None"/>.
        /// </summary>
        public MessageBoxResult DefaultResult { get; init; }

        /// <summary>
        /// Gets the <see cref="MessageBoxImage"/> value that specifies the icon to display.
        /// Default value is <see cref="MessageBoxImage.None"/>.
        /// </summary>
        public MessageBoxImage Icon { get; init; }

        /// <summary>
        /// Gets the string that specifies the text to display.
        /// </summary>
        public string MessageBoxText { get; init; } = "";

        private void SetIfNotNullOrWhiteSpace(ref string field, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            field = value;
        }
    }
}
