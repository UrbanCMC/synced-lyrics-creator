using System;
using System.Linq;
using SyncedLyricsCreator.Data.Models;
using TagLib.Id3v2;

namespace SyncedLyricsCreator.Data.Services
{
    /// <summary>
    /// Converts lyrics between the internally used <see cref="Lyrics"/> and the ID3 <see cref="SynchronisedLyricsFrame"/>
    /// </summary>
    public static class LyricsConverter
    {
        /// <summary>
        /// Loads a lyrics object from an ID3 tag object
        /// </summary>
        /// <param name="tag">The ID3 tag object to load from</param>
        /// <returns>A <see cref="Lyrics"/> object containing the loaded lines</returns>
        /// <remarks>
        /// For now, only lyrics frames for english text will be read from
        /// </remarks>
        public static Lyrics LoadFromTag(Tag tag)
        {
            var lyrics = new Lyrics();
            var lyricsFrame = SynchronisedLyricsFrame.Get(tag, "", "eng", SynchedTextType.Lyrics, true);

            lyrics.Lines = lyricsFrame.Text.Select(s => new Lyrics.LyricsLine
            {
                Timestamp = TimeSpan.FromMilliseconds(s.Time),
                Text = s.Text,
            }).ToList();

            return lyrics;
        }

        /// <summary>
        /// Writes lyrics to an ID3 tag object, overwriting existing ones if present.
        /// </summary>
        /// <param name="lyrics">The synchronized lyrics to save</param>
        /// <returns>The created ID3 frame containing the synchronized lyrics</returns>
        /// <remarks>
        /// For now we always use english as the target language for our frame
        /// </remarks>
        public static SynchronisedLyricsFrame SaveToFrame(Lyrics lyrics)
            => new("", "eng", SynchedTextType.Lyrics)
            {
                Text = lyrics.Lines
                    .Select(s => new SynchedText((long)s.Timestamp.TotalMilliseconds, s.Text))
                    .ToArray(),
            };
    }
}
