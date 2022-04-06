using System;
using System.Timers;
using System.Windows.Input;
using ReactiveUI;
using SyncedLyricsCreator.Audio;

namespace SyncedLyricsCreator.ViewModels
{
    /// <summary>
    /// View model implementation for the player component
    /// </summary>
    public class PlayerViewModel : ViewModelBase, IDisposable
    {
        private readonly Timer refreshTimer;

        private AudioPlayer? audioPlayer;
        private bool isPlaying;
        private double relativeTrackPosition;
        private TimeSpan trackLength;
        private TimeSpan trackPosition;
        private float volume;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerViewModel"/> class
        /// </summary>
        public PlayerViewModel()
        {
            refreshTimer = new Timer
            {
                Interval = 500,
            };

            refreshTimer.Start();
            refreshTimer.Elapsed += RefreshTimer_Elapsed;

            Volume = .5f;
            TrackLength = TimeSpan.Zero;
            TrackPosition = TimeSpan.Zero;

            SetTrackPositionCommand = ReactiveCommand.Create<double>(SetTrackPosition);
            SetVolumeCommand = ReactiveCommand.Create<double>(SetVolume);
            StopPlaybackCommand = ReactiveCommand.Create(StopPlayback, this.WhenAnyValue(x => x.IsPlaying));
            TogglePlayPauseCommand = ReactiveCommand.Create(TogglePlayPause);
        }

        /// <summary>
        /// Gets a value indicating whether the player is currently playing a file
        /// </summary>
        public bool IsPlaying
        {
            get => isPlaying;
            private set => this.RaiseAndSetIfChanged(ref isPlaying, value);
        }

        /// <summary>
        /// Gets or sets a relative value indicating the current track position on a scale of 0 to 100
        /// </summary>
        public double RelativeTrackPosition
        {
            get => relativeTrackPosition;
            set => this.RaiseAndSetIfChanged(ref relativeTrackPosition, value);
        }

        /// <summary>
        /// Gets the command for setting the playback position in the track
        /// </summary>
        public ICommand SetTrackPositionCommand { get; }

        /// <summary>
        /// Gets the command for setting the playback volume
        /// </summary>
        public ICommand SetVolumeCommand { get; }

        /// <summary>
        /// Gets the command for stopping playback
        /// </summary>
        public ICommand StopPlaybackCommand { get; }

        /// <summary>
        /// Gets the command for toggling between play/pause
        /// </summary>
        public ICommand TogglePlayPauseCommand { get; }

        /// <summary>
        /// Gets or sets the total length of the loaded audio track
        /// </summary>
        public TimeSpan TrackLength
        {
            get => trackLength;
            set => this.RaiseAndSetIfChanged(ref trackLength, value);
        }

        /// <summary>
        /// Gets or sets the current position in the loaded audio track
        /// </summary>
        public TimeSpan TrackPosition
        {
            get => trackPosition;
            set
            {
                this.RaiseAndSetIfChanged(ref trackPosition, value);
                this.RaisePropertyChanged(nameof(TrackPositionText));

                if (value == TimeSpan.Zero || TrackLength == TimeSpan.Zero)
                {
                    RelativeTrackPosition = 0;
                }
                else
                {
                    RelativeTrackPosition = value.TotalSeconds / TrackLength.TotalSeconds * 100;
                }
            }
        }

        /// <summary>
        /// Gets a string representation of the total/elapsed playback time of the loaded file
        /// </summary>
        public string TrackPositionText
        {
            get => $"{TrackPosition.TotalMinutes:N0}:{TrackPosition.Seconds:D2} / {TrackLength.TotalMinutes:N0}:{TrackLength.Seconds:D2}";
        }

        /// <summary>
        /// Gets or sets the volume audio is played at
        /// </summary>
        public float Volume
        {
            get => volume;
            set
            {
                this.RaiseAndSetIfChanged(ref volume, value);
                this.RaisePropertyChanged(nameof(VolumeText));

                if (audioPlayer != null)
                {
                    audioPlayer.Volume = value;
                }
            }
        }

        /// <summary>
        /// Gets a string representation of the current volume
        /// </summary>
        public string VolumeText
        {
            get => $"{Volume:P}";
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            audioPlayer?.Dispose();
            audioPlayer = null;
        }

        /// <summary>
        /// Loads a new file to play
        /// </summary>
        public void LoadFile()
        {
            // TODO: Actually load an audio file
            audioPlayer = new AudioPlayer("", Volume);
            audioPlayer.OnPlaybackStateChanged += AudioPlayer_OnPlaybackStateChanged;

            TrackLength = audioPlayer.Duration;
        }

        private void AudioPlayer_OnPlaybackStateChanged()
        {
            if (audioPlayer == null)
            {
                return;
            }

            IsPlaying = audioPlayer.PlaybackState == Audio.Enums.PlaybackState.Playing;
        }

        private void RefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (audioPlayer == null)
            {
                return;
            }

            TrackPosition = audioPlayer.Position;
        }

        private void SetTrackPosition(double relativePosition)
        {
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Position = TimeSpan.FromSeconds(TrackLength.TotalSeconds * relativePosition);
        }

        private void SetVolume(double volume)
        {
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Volume = (float)volume;
        }

        private void StopPlayback()
        {
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Stop();
        }

        private void TogglePlayPause()
        {
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.TogglePlayPause();
        }
    }
}
