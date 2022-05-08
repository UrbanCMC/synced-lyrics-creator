using System;

namespace SyncedLyricsCreator.Events
{
    /// <summary>
    /// Arguments for the ResolveGetPlaybackTimestamp event
    /// </summary>
    public class ResolveGetPlaybackTimestampEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveGetPlaybackTimestampEventArgs"/> class
        /// </summary>
        /// <param name="playbackTime">The current playback time</param>
        public ResolveGetPlaybackTimestampEventArgs(TimeSpan playbackTime) => PlaybackTime = playbackTime;

        /// <summary>
        /// Gets the current playback time
        /// </summary>
        public TimeSpan PlaybackTime { get; }
    }
}
