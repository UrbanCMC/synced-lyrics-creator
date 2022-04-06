using System;
using System.Collections.Generic;

namespace SyncedLyricsCreator.Data.Models
{
    /// <summary>
    /// Represents the synchronized lyrics for a song
    /// </summary>
    public class Lyrics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lyrics"/> class
        /// </summary>
        public Lyrics() => Lines = new List<LyricsLine>();

        /// <summary>
        /// Gets or sets a list of lines, each of which is timestamped
        /// </summary>
        public List<LyricsLine> Lines { get; set; }

        /// <summary>
        /// Represents a single lyrics line
        /// </summary>
        public struct LyricsLine
        {
            /// <summary>
            /// The timestamp indicating when the line should be displayed
            /// </summary>
            public TimeSpan Timestamp;

            /// <summary>
            /// The text to display
            /// </summary>
            public string Text;
        }
    }
}
