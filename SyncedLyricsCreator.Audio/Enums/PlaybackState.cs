namespace SyncedLyricsCreator.Audio.Enums
{
    /// <summary>
    /// An enumeration of possible playback states for the audio player
    /// </summary>
    public enum PlaybackState
    {
        /// <summary>
        /// The playback has been paused and can be resumed from the same position
        /// </summary>
        Paused,

        /// <summary>
        /// The audio player is currently playing the loaded file
        /// </summary>
        Playing,

        /// <summary>
        /// The playback has been stopped.
        /// </summary>
        Stopped,
    }
}
