using System.IO;
using System.Reflection;

namespace SyncedLyricsCreator.Grammars;

/// <summary>
/// Defines a resource loader for TextMate grammars and themes
/// </summary>
internal static class ResourceLoader
{
    private const string GrammarPrefix = "SyncedLyricsCreator.Assets.Grammars.";
    private const string ThemesPrefix = "SyncedLyricsCreator.Assets.Themes.";

    /// <summary>
    /// Opens a stream for the grammar package with the specified name
    /// </summary>
    /// <param name="grammarName">The name of the grammar to open</param>
    /// <returns>A stream of the opened grammar package</returns>
    /// <exception cref="FileNotFoundException">Thrown if no grammar package with the specified name can be found</exception>
    internal static Stream OpenGrammarPackage(string grammarName)
    {
        var grammarPackage = GrammarPrefix + grammarName.ToLowerInvariant() + ".package.json";
        var result = typeof(ResourceLoader).GetTypeInfo().Assembly.GetManifestResourceStream(grammarPackage);
        if (result == null)
        {
            throw new FileNotFoundException("The grammar package '" + grammarPackage + "' was not found.");
        }

        return result;
    }

    /// <summary>
    /// Tries to open a stream for the specified grammar resource
    /// </summary>
    /// <param name="path">The path to the package to open</param>
    /// <returns>The opened <see cref="Stream"/>, or <c>null</c> if the resource wasn't found</returns>
    internal static Stream? TryOpenGrammarStream(string path)
    {
        return typeof(ResourceLoader).GetTypeInfo().Assembly.GetManifestResourceStream(GrammarPrefix + path)
            ?? null;
    }

    /// <summary>
    /// Tries to open a stream for the specified theme resource
    /// </summary>
    /// <param name="path">The path to the theme to open</param>
    /// <returns>The opened <see cref="Stream"/>, or <c>null</c> if the resource wasn't found</returns>
    internal static Stream? TryOpenThemeStream(string path)
    {
        return typeof(ResourceLoader).GetTypeInfo().Assembly.GetManifestResourceStream(ThemesPrefix + path)
            ?? null;
    }
}
