using System;

namespace SyncedLyricsCreator.Events
{
    /// <summary>
    /// Arguments for the InitiateGetPlaybackTimestamp event
    /// </summary>
    public class InitiateGetPlaybackTimestampEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitiateGetPlaybackTimestampEventArgs"/> class
        /// </summary>
        public InitiateGetPlaybackTimestampEventArgs() => RequestTime = DateTime.Now;

        /// <summary>
        /// Gets the time when the request for the current playback timestamp was sent
        /// </summary>
        /// <remarks>
        /// This is an experimental attempt to work against any delays introduced by the in-app communication
        /// </remarks>
        public DateTime RequestTime { get; }
    }
}
