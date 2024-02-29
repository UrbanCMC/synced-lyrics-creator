using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyncedLyricsCreator.Data.Enums;
using SyncedLyricsCreator.Data.Models;
using SyncedLyricsCreator.Data.Web;

namespace SyncedLyricsCreator.Data.Services;

/// <summary>
/// Allows searching for lyrics on different services
/// </summary>
public static class LyricsSearchService
{
    /// <summary>
    /// Asynchronously searches for lyrics on the specified API
    /// </summary>
    /// <param name="artist">The name of the artist</param>
    /// <param name="album">The name of the album</param>
    /// <param name="track">The title of the track</param>
    /// <param name="searchApi">The API to search</param>
    /// <returns>A list of search results</returns>
    /// <exception cref="ArgumentException">Thrown if the specified API is not supported</exception>
    public static async Task<List<LyricsSearchResult>> Search(string artist, string album, string track, LyricsSearchApi searchApi)
    {
        switch (searchApi)
        {
            case LyricsSearchApi.LrcLib:
                return await LrcLibApi.SearchAsync(artist, album, track);
        }

        throw new ArgumentException("Specified search API is not supported", nameof(searchApi));
    }
}
