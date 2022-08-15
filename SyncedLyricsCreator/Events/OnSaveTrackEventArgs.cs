using System;

namespace SyncedLyricsCreator.Events
{
    /// <summary>
    /// Arguments for the OnSaveTrack event
    /// </summary>
    public class OnSaveTrackEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnSaveTrackEventArgs"/> class
        /// </summary>
        /// <param name="path">The path to the file to save</param>
        public OnSaveTrackEventArgs(string path) => Path = path;

        /// <summary>
        /// Gets the path to the file to save
        /// </summary>
        public string Path { get; }
    }
}
