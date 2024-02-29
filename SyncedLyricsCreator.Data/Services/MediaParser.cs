using SyncedLyricsCreator.Data.Models;
using TagLib;
using File = System.IO.File;

namespace SyncedLyricsCreator.Data.Services;

/// <summary>
/// Parses media files to extract information about the music track
/// </summary>
public static class MediaParser
{
    public static MediaInfo? GetTrackInfo(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var tag = TagLib.File.Create(path).GetTag(TagTypes.Id3v2, false);
        if (tag == null)
        {
            return null;
        }

        return new MediaInfo { Artist = tag.FirstAlbumArtist, Album = tag.Album, Track = tag.Title };
    }
}
