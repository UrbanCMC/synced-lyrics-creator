using System;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;
using SyncedLyricsCreator.Data.Enums;
using SyncedLyricsCreator.Data.Models;
using SyncedLyricsCreator.Data.Services;
using SyncedLyricsCreator.Helpers.Dialog;

namespace SyncedLyricsCreator.ViewModels.Dialogs;

/// <summary>
/// The view model for the lyrics search view
/// </summary>
public class LyricsSearchViewModel : ViewModelBase, IModalDialogViewModel
{
    private bool? dialogResult;
    private bool isComplete;
    private string searchArtist;
    private string searchAlbum;
    private string searchTrack;
    private LyricsSearchApi selectedSearchApi;
    private LyricsSearchResult? selectedSearchResult;

    /// <summary>
    /// Initializes a new instance of the <see cref="LyricsSearchViewModel"/> class
    /// </summary>
    /// <param name="currentFilePath">The path to the currently opened music file</param>
    public LyricsSearchViewModel(string currentFilePath)
    {
        SearchCommand = ReactiveCommand.CreateFromTask(Search);

        var mediaInfo = MediaParser.GetTrackInfo(currentFilePath);
        if (mediaInfo != null)
        {
            SearchAlbum = mediaInfo.Album;
            SearchArtist = mediaInfo.Artist;
            SearchTrack = mediaInfo.Track;
        }
    }

    /// <inheritdoc />
    public bool? DialogResult
    {
        get => dialogResult;
        private set => this.RaiseAndSetIfChanged(ref dialogResult, value);
    }

    /// <inheritdoc />
    public bool IsComplete
    {
        get => isComplete;
        private set => this.RaiseAndSetIfChanged(ref isComplete, value);
    }

    /// <summary>
    /// Gets or sets the artist to search for
    /// </summary>
    public string SearchArtist
    {
        get => searchArtist;
        set => this.RaiseAndSetIfChanged(ref searchArtist, value);
    }

    /// <summary>
    /// Gets a list of results returned by the API
    /// </summary>
    public ObservableCollection<LyricsSearchResult> SearchResults { get; } = new();

    /// <summary>
    /// Gets or sets the album to search for
    /// </summary>
    public string SearchAlbum
    {
        get => searchAlbum;
        set => this.RaiseAndSetIfChanged(ref searchAlbum, value);
    }

    /// <summary>
    /// Gets a list of APIs that can be used for searching
    /// </summary>
    public ObservableCollection<LyricsSearchApi> SearchApis { get; } = new(Enum.GetValues(typeof(LyricsSearchApi)).Cast<LyricsSearchApi>());

    /// <summary>
    /// Gets the command for the Search button
    /// </summary>
    public ICommand SearchCommand { get; }

    /// <summary>
    /// Gets or sets the track to search for
    /// </summary>
    public string SearchTrack
    {
        get => searchTrack;
        set => this.RaiseAndSetIfChanged(ref searchTrack, value);
    }

    /// <summary>
    /// Gets or sets the selected search API
    /// </summary>
    public LyricsSearchApi SelectedSearchApi
    {
        get => selectedSearchApi;
        set => this.RaiseAndSetIfChanged(ref selectedSearchApi, value);
    }

    public LyricsSearchResult? SelectedSearchResult
    {
        get => selectedSearchResult;
        set => this.RaiseAndSetIfChanged(ref selectedSearchResult, value);
    }

    public void SelectResult()
    {
        DialogResult = SelectedSearchResult != null;
        IsComplete = true;
    }

    private async Task Search()
    {
        SelectedSearchResult = null;
        var results = await LyricsSearchService.Search(SearchArtist, SearchAlbum, SearchTrack, SelectedSearchApi);

        // TODO: Show some indicator of how many results were returned, if any
        SearchResults.Clear();
        SearchResults.AddRange(results);
    }
}
