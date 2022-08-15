using System;

namespace SyncedLyricsCreator.Events
{
    /// <summary>
    /// Arguments for the OnLoadTrack event
    /// </summary>
    public class OnLoadTrackEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnLoadTrackEventArgs"/> class
        /// </summary>
        /// <param name="path">The path to the file that was loaded</param>
        public OnLoadTrackEventArgs(string path) => Path = path;

        /// <summary>
        /// Gets the path to the loaded file
        /// </summary>
        public string Path { get; }
    }
}
