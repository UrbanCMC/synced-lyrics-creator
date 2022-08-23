using System;
using System.Linq;
using SyncedLyricsCreator.Data.Models;
using TagLib;
using TagLib.Id3v2;
using File = System.IO.File;
using Tag = TagLib.Id3v2.Tag;

namespace SyncedLyricsCreator.Data.Services
{
    /// <summary>
    /// Converts lyrics between the internally used <see cref="Lyrics"/> and the ID3 <see cref="SynchronisedLyricsFrame"/>
    /// </summary>
    public static class LyricsConverter
    {
        /// <summary>
        /// Loads the lyrics from the specified file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>A <see cref="Lyrics"/> object containing the file's lyrics, or <c>null</c> if the file couldn't be loaded.</returns>
        public static Lyrics? LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var lyrics = new Lyrics();

            try
            {
                var file = TagLib.File.Create(path);
                return file?.GetTag(TagTypes.Id3v2, true) is not Tag tag ? lyrics : LoadFromTag(tag);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Saves the specified lyrics to the file
        /// </summary>
        /// <param name="path">The path to the target file</param>
        /// <param name="lyrics">The lyrics to save</param>
        public static void SaveToFile(string path, Lyrics lyrics)
        {
            var file = TagLib.File.Create(path);
            var frame = SaveToFrame(lyrics);

            var tag = file.GetTag(TagTypes.Id3v2, true) as Tag;
            var existingFrame = GetFrame(tag!);
            tag!.ReplaceFrame(existingFrame, frame);

            file.Save();
        }

        private static SynchronisedLyricsFrame GetFrame(Tag tag) => SynchronisedLyricsFrame.Get(tag, "", "eng", SynchedTextType.Lyrics, true);

        /// <summary>
        /// Loads a lyrics object from an ID3 tag object
        /// </summary>
        /// <param name="tag">The ID3 tag object to load from</param>
        /// <returns>A <see cref="Lyrics"/> object containing the loaded lines</returns>
        /// <remarks>
        /// For now, only lyrics frames for english text will be read from
        /// </remarks>
        private static Lyrics LoadFromTag(Tag tag)
        {
            var lyrics = new Lyrics();
            var lyricsFrame = GetFrame(tag);

            lyrics.Lines = lyricsFrame.Text.Select(s => new Lyrics.LyricsLine { Timestamp = TimeSpan.FromMilliseconds(s.Time), Text = s.Text, }).ToList();

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
        private static SynchronisedLyricsFrame SaveToFrame(Lyrics lyrics)
            => new("", "eng", SynchedTextType.Lyrics)
            {
                Text = lyrics.Lines
                    .Select(s => new SynchedText((long)s.Timestamp.TotalMilliseconds, s.Text))
                    .ToArray()
                , Format = TimestampFormat.AbsoluteMilliseconds
            };
    }
}
