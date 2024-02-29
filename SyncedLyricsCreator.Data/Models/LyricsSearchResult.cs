namespace SyncedLyricsCreator.Data.Models;

public class LyricsSearchResult
{
    public string Album { get; set; }
    public string Artist { get; set; }
    public string Track { get; set; }
    public int Duration { get; set; }
    public bool IsInstrumental { get; set; }
    public string PlainLyrics { get; set; }
    public string SyncedLyrics { get; set; }
}
