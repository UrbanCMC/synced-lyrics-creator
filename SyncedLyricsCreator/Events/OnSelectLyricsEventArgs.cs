using System;

namespace SyncedLyricsCreator.Events;

/// <summary>
/// Arguments for the OnSelectLyrics event
/// </summary>
public class OnSelectLyricsEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OnSelectLyricsEventArgs"/> class
    /// </summary>
    /// <param name="lyrics">The lyrics that have been selected</param>
    public OnSelectLyricsEventArgs(string lyrics) => Lyrics = lyrics;

    /// <summary>
    /// Gets the lyrics that have been selected
    /// </summary>
    public string Lyrics { get; }
}
