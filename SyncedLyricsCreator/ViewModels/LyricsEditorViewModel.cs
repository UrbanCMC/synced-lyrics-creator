using System;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using ReactiveUI;
using SyncedLyricsCreator.Data.Models;
using SyncedLyricsCreator.Events;

namespace SyncedLyricsCreator.ViewModels
{
    /// <summary>
    /// View model implementation for the lyrics editor component
    /// </summary>
    public class LyricsEditorViewModel : ViewModelBase
    {
        private readonly string[] timestampFormats =
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

        /// <summary>
        /// Gets the current lyrics formatted as a list
        /// </summary>
        /// <returns>The lyrics currently in the editor</returns>
        public Lyrics GetLyrics()
        {
            var lyrics = new Lyrics();

            var lines = EditorText.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var timestamp = GetTimestampForLine(line);
                if (timestamp == null)
                {
                    continue;
                }

                var endOfTimestampIndex = EditorText.IndexOf(']') + 1;
                lyrics.Lines.Add(new Lyrics.LyricsLine { Timestamp = timestamp.Value, Text = line[endOfTimestampIndex..].Trim() });
            }

            return lyrics;
        }

        /// <summary>
        /// Loads the specified lyrics into the editor
        /// </summary>
        /// <param name="lyrics">The lyrics to load</param>
        public void Load(Lyrics lyrics)
        {
            var sb = new StringBuilder();
            foreach (var line in lyrics.Lines)
            {
                sb.Append(line.Timestamp.ToString(timestampFormats[0])).AppendLine(line.Text);
            }

            EditorText = sb.ToString();
        }

        private void RequestPlaybackTime()
            => MessageBus.Current.SendMessage(new InitiateGetPlaybackTimestampEventArgs());

        private void InsertPlaybackTime(ResolveGetPlaybackTimestampEventArgs args)
        {
            var playbackTime = args.PlaybackTime;
            var lineStartIndex = GetCurrentLineStartIndex();

            // Remove current timestamp from line
            if (GetCurrentLineTimestamp() != null)
            {
                var endOfTimestampIndex = EditorText.IndexOf(']', lineStartIndex) + 1;
                EditorText = EditorText.Remove(lineStartIndex, endOfTimestampIndex - lineStartIndex);
            }

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

            var lineStart = textUntilCursor.LastIndexOf(Environment.NewLine, StringComparison.OrdinalIgnoreCase);
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

            return GetTimestampForLine(lineFromCursor);
        }

        private TimeSpan? GetTimestampForLine(string line)
        {
            var eolIndex = line.IndexOf("]", StringComparison.OrdinalIgnoreCase);
            if (eolIndex == -1)
            {
                return null;
            }

            line = line[..(eolIndex + 1)];
            var textToParse = line.TrimStart();
            foreach (var format in timestampFormats)
            {
                if (TimeSpan.TryParseExact(textToParse, format, CultureInfo.InvariantCulture, out var timestamp))
                {
                    return timestamp;
                }
            }

            return null;
        }
    }
}
