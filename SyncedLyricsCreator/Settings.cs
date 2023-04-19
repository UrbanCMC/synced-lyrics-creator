using System.IO;
using System.Reflection;
using Avalonia.Input;
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

        #region Editor

        /// <summary>
        /// Gets or sets a value indicating whether the editor should automatically advance
        /// to the next line after syncing the current one
        /// </summary>
        public bool AdvanceLineAfterSyncing { get; set; }

        #endregion

        #region Hotkeys

        /// <summary>
        /// Gets or sets the gesture to decrease the timestamp for the current line
        /// </summary>
        public KeyGesture DecreaseTimestampKeyBinding { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gesture to increase the timestamp for the current line
        /// </summary>
        public KeyGesture IncreaseTimestampKeyBinding { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gesture to jump to the current timestamp in the loaded music file
        /// </summary>
        public KeyGesture JumpToTimestampKeyBinding { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gesture to set a timestamp for the current line
        /// </summary>
        public KeyGesture SetTimestampKeyBinding { get; set; } = null!;

        #endregion

        #region Timestamps

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
            AdvanceLineAfterSyncing = false;

            DecreaseTimestampKeyBinding = KeyGesture.Parse("Ctrl+Down");
            IncreaseTimestampKeyBinding = KeyGesture.Parse("Ctrl+Up");
            JumpToTimestampKeyBinding = KeyGesture.Parse("F6");
            SetTimestampKeyBinding = KeyGesture.Parse("F7");

            RoundTimestampMsToHundredths = true;
            TimestampDelayMs = 0;
        }
    }
}
