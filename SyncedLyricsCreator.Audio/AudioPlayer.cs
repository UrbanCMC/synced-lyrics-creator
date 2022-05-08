using System;
using NAudio.Wave;
using SyncedLyricsCreator.Audio.Enums;

namespace SyncedLyricsCreator.Audio
{
    /// <summary>
    /// Abstraction for NAudio audio playback functionality
    /// </summary>
    public class AudioPlayer : IDisposable
    {
        private WasapiOut? audioOut;
        private AudioFileReader? audioReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioPlayer"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file that should be played</param>
        /// <param name="volume">The volume at which the file should be played</param>
        public AudioPlayer(string filePath, float volume)
        {
            PlaybackState = Enums.PlaybackState.Stopped;
            PlaybackStoppedReason = PlaybackStoppedReason.StoppedByEOF;

            audioOut = new WasapiOut();
            audioOut.PlaybackStopped += AudioOut_PlaybackStopped;

            audioReader = new AudioFileReader(filePath)
            {
                Volume = NormalizeVolume(volume),
            };

            var waveChannel = new WaveChannel32(audioReader)
            {
                PadWithZeroes = false,
            };

            audioOut.Init(waveChannel);
        }

        /// <summary>
        /// The event raised when the audio playback state is changed
        /// </summary>
        public event Action? OnPlaybackStateChanged;

        /// <summary>
        /// Gets the reason why playback was stopped
        /// </summary>
        public PlaybackStoppedReason PlaybackStoppedReason { get; private set; }

        /// <summary>
        /// Gets the playback state of the audio player
        /// </summary>
        public Enums.PlaybackState PlaybackState { get; private set; }

        /// <summary>
        /// Gets the total duration of the audio file, or <c>TimeSpan.Zero</c> if no file is loaded
        /// </summary>
        public TimeSpan Duration
        {
            get => audioReader?.TotalTime ?? TimeSpan.Zero;
        }

        /// <summary>
        /// Gets or sets the current playback position of the audio file, or <c>TimeSpan.Zero</c> if no file is loaded
        /// </summary>
        public TimeSpan Position
        {
            get => audioReader?.CurrentTime ?? TimeSpan.Zero;
            set
            {
                if (audioReader != null)
                {
                    audioReader.CurrentTime = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current volume of the audio player
        /// </summary>
        /// <remarks>
        /// Input values will be truncated to between 0 and 1.
        /// </remarks>
        public float Volume
        {
            get => audioReader?.Volume ?? 1;
            set
            {
                if (audioReader != null)
                {
                    audioReader.Volume = NormalizeVolume(value);
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (audioOut?.PlaybackState == NAudio.Wave.PlaybackState.Playing)
            {
                audioOut.Stop();
            }

            audioOut?.Dispose();
            audioOut = null;

            audioReader?.Dispose();
            audioReader = null;
        }

        /// <summary>
        /// Stops the playback of the current audio file
        /// </summary>
        public void Stop()
        {
            PlaybackStoppedReason = PlaybackStoppedReason.StoppedByUser;
            audioOut?.Stop();
        }

        /// <summary>
        /// Toggles the playback of the current audio file between playing/paused
        /// </summary>
        public void TogglePlayPause()
        {
            if (audioOut == null)
            {
                return;
            }

            if (audioOut.PlaybackState == NAudio.Wave.PlaybackState.Playing)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        private static float NormalizeVolume(float inputVolume)
        {
            if (inputVolume < 0)
            {
                return 0;
            }
            else if (inputVolume > 1)
            {
                return 1;
            }

            return inputVolume;
        }

        private void AudioOut_PlaybackStopped(object? sender, StoppedEventArgs? e)
        {
            PlaybackState = Enums.PlaybackState.Stopped;

            // Check whether user manually stopped playback
            if (PlaybackStoppedReason != PlaybackStoppedReason.StoppedByUser)
            {
                PlaybackStoppedReason = PlaybackStoppedReason.StoppedByEOF;
            }

            OnPlaybackStateChanged?.Invoke();
        }

        private void Pause()
        {
            audioOut?.Pause();

            PlaybackState = Enums.PlaybackState.Paused;

            OnPlaybackStateChanged?.Invoke();
        }

        private void Play()
        {
            audioOut?.Play();

            PlaybackStoppedReason = PlaybackStoppedReason.None;
            PlaybackState = Enums.PlaybackState.Playing;

            OnPlaybackStateChanged?.Invoke();
        }
    }
}
