using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TextMateSharp.Grammars;
using TextMateSharp.Internal.Grammars.Reader;
using TextMateSharp.Internal.Themes.Reader;
using TextMateSharp.Internal.Types;
using TextMateSharp.Registry;
using TextMateSharp.Themes;

namespace SyncedLyricsCreator.Grammars;

/// <summary>
/// Defines the TextMate registry options for the application
/// </summary>
public class RegistryOptions : IRegistryOptions
{
    private readonly Dictionary<string, GrammarDefinition> availableGrammars = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="RegistryOptions"/> class
    /// </summary>
    public RegistryOptions()
    {
        InitializeAvailableGrammars();
    }

    /// <inheritdoc />
    public IRawTheme GetTheme(string scopeName)
    {
        var themeStream = ResourceLoader.TryOpenThemeStream(scopeName.Replace("./", string.Empty));
        if (themeStream == null)
        {
            return null;
        }

        using (themeStream)
        {
            using var reader = new StreamReader(themeStream);
            return ThemeReader.ReadThemeSync(reader);
        }
    }

    /// <inheritdoc />
    public IRawGrammar GetGrammar(string scopeName)
    {
        var grammarStream = ResourceLoader.TryOpenGrammarStream(GetGrammarFile(scopeName));
        if (grammarStream == null)
        {
            return null;
        }

        using (grammarStream)
        {
            using var reader = new StreamReader(grammarStream);
            return GrammarReader.ReadGrammarSync(reader);
        }
    }

    /// <inheritdoc />
    public ICollection<string> GetInjections(string scopeName)
    {
        return new List<string>();
    }

    /// <inheritdoc />
    public IRawTheme GetDefaultTheme()
    {
        return LoadTheme();
    }

    private static string GetThemeFile()
    {
        return "dark_vs.json";
    }

    private string GetGrammarFile(string scopeName)
    {
        foreach (var name in availableGrammars.Keys)
        {
            var definition = availableGrammars[name];
            foreach (var grammar in definition.Contributes.Grammars)
            {
                if (!scopeName.Equals(grammar.ScopeName))
                {
                    continue;
                }

                var grammarPath = grammar.Path;
                if (grammarPath.StartsWith("./"))
                {
                    grammarPath = grammarPath[2..];
                }

                grammarPath = grammarPath.Replace("/", ".");
                return name.ToLower() + "." + grammarPath;
            }
        }

        return null;
    }

    private void InitializeAvailableGrammars()
    {
        var serializer = new JsonSerializer();
        foreach (var grammar in GrammarNames.SupportedGrammars)
        {
            using var stream = ResourceLoader.OpenGrammarPackage(grammar);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            var definition = serializer.Deserialize<GrammarDefinition>(jsonReader);
            availableGrammars.Add(grammar, definition);
        }
    }

    private IRawTheme LoadTheme()
    {
        return GetTheme(GetThemeFile());
    }
}
