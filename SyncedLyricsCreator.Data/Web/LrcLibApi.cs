using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using SyncedLyricsCreator.Data.Models;

namespace SyncedLyricsCreator.Data.Web;

/// <summary>
/// Provides access to the lrclib.net API
/// </summary>
public static class LrcLibApi
{
    private const string LrcLibApiUrl = "https://lrclib.net/api/";

    /// <summary>
    /// Asynchronously searches for lyrics based on the specified parameters
    /// </summary>
    /// <param name="artist">The name of the artist</param>
    /// <param name="album">The name of the album</param>
    /// <param name="track">The title of the track</param>
    /// <returns>A list of search results matching the specified parameters</returns>
    public static async Task<List<LyricsSearchResult>> SearchAsync(string artist, string album, string track)
    {
        var results = new List<LyricsSearchResult>();

        try
        {
            using (var client = new RestClient(LrcLibApiUrl))
            {
                var request = new RestRequest("search");
                request.AddQueryParameter("track_name", track);
                if (!string.IsNullOrWhiteSpace(artist))
                {
                    request.AddQueryParameter("artist_name", artist);
                }

                if (!string.IsNullOrWhiteSpace(album))
                {
                    request.AddQueryParameter("album_name", album);
                }

                var response = await client.GetAsync(request);
                var doc = JsonDocument.Parse(response.Content ?? "{}");

                if (doc.RootElement.GetArrayLength() <= 0)
                {
                    return results;
                }

                foreach (var result in doc.RootElement.EnumerateArray())
                {
                    var searchResult = new LyricsSearchResult()
                    {
                        Album = result.GetProperty("albumName").GetString() ?? ""
                        , Artist = result.GetProperty("artistName").GetString() ?? ""
                        , Track = result.GetProperty("trackName").GetString() ?? ""
                        , Duration = Convert.ToInt32(result.GetProperty("duration").GetSingle())
                        , IsInstrumental = result.GetProperty("instrumental").GetBoolean()
                        , PlainLyrics = result.GetProperty("plainLyrics").GetString() ?? ""
                        , SyncedLyrics = result.GetProperty("syncedLyrics").GetString() ?? ""
                    };

                    results.Add(searchResult);
                }

                return results;
            }
        }
        catch (Exception ex)
        {
            return results;
        }
    }
}
