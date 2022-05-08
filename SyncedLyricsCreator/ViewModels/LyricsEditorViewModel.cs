using System;
using System.Globalization;
using System.Windows.Input;
using ReactiveUI;
using SyncedLyricsCreator.Events;

namespace SyncedLyricsCreator.ViewModels
{
    /// <summary>
    /// View model implementation for the lyrics editor component
    /// </summary>
    public class LyricsEditorViewModel : ViewModelBase
    {
        private readonly string[] timestampFormats = new[]
        {
            @"\[mm\:ss\.fff\]",
            @"\[mm\:ss\.ff\]",
            @"\[m\:ss\.fff\]",
            @"\[m\:ss\.ff\]",
        };

        private int cursorIndex;
        private string editorText;

        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsEditorViewModel"/> class
        /// </summary>
        public LyricsEditorViewModel()
        {
            // TODO: Initialize correctly
            editorText = "";
            EditorText = "";

            // Subscribe to events
            MessageBus.Current.Listen<ResolveGetPlaybackTimestampEventArgs>()
                .Subscribe(InsertPlaybackTime);

            JumpToTimestampCommand = ReactiveCommand.Create(JumpToTimestamp);
            SetTimestampCommand = ReactiveCommand.Create(RequestPlaybackTime);
        }

        /// <summary>
        /// Gets or sets the position of the cursor in the lyrics text
        /// </summary>
        public int CursorIndex
        {
            get => cursorIndex;
            set => this.RaiseAndSetIfChanged(ref cursorIndex, value);
        }

        /// <summary>
        /// Gets or sets the lyrics text that is being edited
        /// </summary>
        public string EditorText
        {
            get => editorText;
            set => this.RaiseAndSetIfChanged(ref editorText, value);
        }

        /// <summary>
        /// Gets the command to jump to the timestamp of the current lyrics line
        /// </summary>
        public ICommand JumpToTimestampCommand { get; }

        /// <summary>
        /// Gets the command to set a timestamp for the current lyrics line
        /// </summary>
        public ICommand SetTimestampCommand { get; }

        private void RequestPlaybackTime()
            => MessageBus.Current.SendMessage(new InitiateGetPlaybackTimestampEventArgs());

        private void InsertPlaybackTime(ResolveGetPlaybackTimestampEventArgs args)
        {
            var playbackTime = args.PlaybackTime;
            var lineStartIndex = GetCurrentLineStartIndex();

            EditorText = EditorText.Insert(lineStartIndex, playbackTime.ToString(timestampFormats[0]));
        }

        private void JumpToTimestamp()
        {
            if (string.IsNullOrWhiteSpace(EditorText))
            {
                return;
            }

            var timestamp = GetCurrentLineTimestamp();
            if (timestamp == null)
            {
                return;
            }

            MessageBus.Current.SendMessage(new JumpToTimestampEventArgs(timestamp.Value));
        }

        private int GetCurrentLineStartIndex()
        {
            var textUntilCursor = EditorText.Substring(0, CursorIndex);
            if (string.IsNullOrWhiteSpace(textUntilCursor))
            {
                return 0;
            }

            var lineStart = textUntilCursor.LastIndexOf(Environment.NewLine);
            if (lineStart > 0)
            {
                // Advance one to get to first character after newline
                lineStart += Environment.NewLine.Length;
            }
            else if (lineStart == -1)
            {
                lineStart = 0;
            }

            return lineStart;
        }

        private TimeSpan? GetCurrentLineTimestamp()
        {
            var lineStartIndex = GetCurrentLineStartIndex();
            var lineFromCursor = EditorText[lineStartIndex..];

            var eolIndex = lineFromCursor.IndexOf("]");
            if (eolIndex == -1)
            {
                return null;
            }

            lineFromCursor = lineFromCursor[..(eolIndex + 1)];
            var textToParse = lineFromCursor.TrimStart();
            for (var i = 0; i < timestampFormats.Length; i++)
            {
                if (TimeSpan.TryParseExact(textToParse, timestampFormats[i], CultureInfo.InvariantCulture, out var timestamp))
                {
                    return timestamp;
                }
            }

            return null;
        }
    }
}
