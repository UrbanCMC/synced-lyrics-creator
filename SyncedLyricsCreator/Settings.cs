using System.IO;
using System.Reflection;
using Westwind.Utilities.Configuration;

namespace SyncedLyricsCreator
{
    /// <summary>
    /// Strongly typed application settings
    /// </summary>
    public class Settings : AppConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings() => InitializeDefaults();

        /// <summary>
        /// Gets the public instance
        /// </summary>
        public static Settings Instance { get; } = new();

        #region Syncing

        /// <summary>
        /// Gets or sets a value indicating whether the milliseconds of timestamps should be
        /// rounded to the hundredths position (e.g. <c>[00:01.123]</c> -> <c>[00:01.120]</c>)
        /// </summary>
        public bool RoundTimestampMsToHundredths { get; set; }

        /// <summary>
        /// Gets or sets the number of milliseconds to subtract from the
        /// actual timestamp when syncing a line to make it easier to get the timing right
        /// </summary>
        public int TimestampDelayMs { get; set; }

        #endregion

        /// <inheritdoc/>
        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            var provider = new JsonFileConfigurationProvider<Settings>
            {
                JsonConfigurationFile = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!
                    , "data"
                    , "Settings.json")
            };

            // Ensure data directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(provider.JsonConfigurationFile)!);

            Provider = provider;
            return provider;
        }

        private void InitializeDefaults()
        {
            RoundTimestampMsToHundredths = true;
            TimestampDelayMs = 0;
        }
    }
}
