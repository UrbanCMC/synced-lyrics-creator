using System;
using System.Timers;
using System.Windows.Input;
using ReactiveUI;
using SyncedLyricsCreator.Audio;
using SyncedLyricsCreator.Audio.Enums;
using SyncedLyricsCreator.Events;

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

            refreshTimer.Elapsed += RefreshTimer_Elapsed;

            Volume = .5f;
            TrackLength = TimeSpan.Zero;
            TrackPosition = TimeSpan.Zero;

            StopPlaybackCommand = ReactiveCommand.Create(StopPlayback, this.WhenAnyValue(x => x.IsPlaying));
            TogglePlayPauseCommand = ReactiveCommand.Create(TogglePlayPause);

            MessageBus.Current.Listen<InitiateGetPlaybackTimestampEventArgs>()
                .Subscribe(PublishPlaybackTime);
            MessageBus.Current.Listen<JumpToTimestampEventArgs>()
                .Subscribe(JumpToTimestamp);
        }

        /// <summary>
        /// Gets a value indicating whether the player is currently playing a file
        /// </summary>
        public bool IsPlaying
        {
            get => isPlaying;
            private set
            {
                this.RaiseAndSetIfChanged(ref isPlaying, value);
                if (value)
                {
                    refreshTimer.Start();
                }
                else
                {
                    refreshTimer.Stop();
                }
            }
        }

        /// <summary>
        /// Gets or sets a relative value indicating the current track position on a scale of 0 to 100
        /// </summary>
        public double RelativeTrackPosition
        {
            get => relativeTrackPosition;
            set
            {
                this.RaiseAndSetIfChanged(ref relativeTrackPosition, value);
                SetTrackPosition(value);
            }
        }

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
        public string TrackPositionText => $"{TrackPosition.Minutes:N0}:{TrackPosition.Seconds:D2} / {TrackLength.Minutes:N0}:{TrackLength.Seconds:D2}";

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
        public string VolumeText => $"{Volume:P}";

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
        /// <param name="filePath">The path to the file</param>
        public void LoadFile(string filePath)
        {
            audioPlayer = new AudioPlayer(filePath, Volume);
            audioPlayer.OnPlaybackStateChanged += AudioPlayer_OnPlaybackStateChanged;

            TrackLength = audioPlayer.Duration;
        }

        private void AudioPlayer_OnPlaybackStateChanged()
        {
            if (audioPlayer == null)
            {
                return;
            }

            IsPlaying = audioPlayer.PlaybackState == PlaybackState.Playing;
        }

        private void JumpToTimestamp(JumpToTimestampEventArgs args)
        {
            if (audioPlayer == null)
            {
                return;
            }

            audioPlayer.Position = args.Timestamp;
        }

        private void PublishPlaybackTime(InitiateGetPlaybackTimestampEventArgs args)
        {
            if (audioPlayer == null)
            {
                return;
            }

            var offset = DateTime.Now - args.RequestTime;
            var playbackTime = TrackPosition - offset;

            MessageBus.Current.SendMessage(new ResolveGetPlaybackTimestampEventArgs(playbackTime));
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

            audioPlayer.Position = TimeSpan.FromSeconds(TrackLength.TotalSeconds * (relativePosition / 100));
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
