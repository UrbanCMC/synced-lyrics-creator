using System;

namespace SyncedLyricsCreator.Events
{
    /// <summary>
    /// Arguments for the JumpToTimestamp event
    /// </summary>
    public class JumpToTimestampEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JumpToTimestampEventArgs"/> class
        /// </summary>
        /// <param name="timestamp">The timestamp the player should jump to</param>
        public JumpToTimestampEventArgs(TimeSpan timestamp) => Timestamp = timestamp;

        /// <summary>
        /// Gets the timestamp the player should jump to
        /// </summary>
        public TimeSpan Timestamp { get; }
    }
}
