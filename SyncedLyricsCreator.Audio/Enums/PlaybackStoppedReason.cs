namespace SyncedLyricsCreator.Audio.Enums
{
    /// <summary>
    /// Defines the ways in which playback of a file can be stopped
    /// </summary>
    public enum PlaybackStoppedReason
    {
        /// <summary>
        /// Playback is currently not stopped
        /// </summary>
        None,

        /// <summary>
        /// Audio playback was stopped manually by the user
        /// </summary>
        StoppedByUser,

        /// <summary>
        /// Audio playback was stopped by reaching the end of the file
        /// </summary>
        StoppedByEOF,
    }
}
