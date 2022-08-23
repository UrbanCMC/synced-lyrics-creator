using System;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SyncedLyricsCreator.Audio
{
    /// <summary>
    /// Implementation of <see cref="AudioStreamReader"/> that can be initialized from a stream
    /// </summary>
    public class AudioStreamReader : WaveStream, ISampleProvider
    {
        private readonly SampleChannel sampleChannel;
        private readonly int destBytesPerSample;
        private readonly int sourceBytesPerSample;
        private readonly object lockObject;
        private WaveStream? readerStream;

        /// <summary>Initializes a new instance of the <see cref="AudioStreamReader"/> class</summary>
        /// <param name="fileName">The name of the file. Used only to determine the correct reader type</param>
        /// <param name="stream">The stream to open</param>
        public AudioStreamReader(string fileName, Stream stream)
        {
            lockObject = new object();
            CreateReaderStream(fileName, stream);
            sourceBytesPerSample = readerStream!.WaveFormat.BitsPerSample / 8 * readerStream.WaveFormat.Channels;
            sampleChannel = new SampleChannel(readerStream, false);
            destBytesPerSample = 4 * sampleChannel.WaveFormat.Channels;
            Length = SourceToDest(readerStream.Length);
        }

        /// <summary>
        /// Gets the  WaveFormat of this stream
        /// </summary>
        public override WaveFormat WaveFormat => sampleChannel.WaveFormat;

        /// <summary>
        /// Gets the length of this stream (in bytes)
        /// </summary>
        public override long Length { get; }

        /// <summary>
        /// Gets or sets the position of this stream (in bytes)
        /// </summary>
        public override long Position
        {
            get => SourceToDest(readerStream!.Position);
            set
            {
                lock (lockObject)
                {
                    readerStream!.Position = DestToSource(value);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Volume of this AudioFileReader. 1.0f is full volume
        /// </summary>
        public float Volume
        {
            get => sampleChannel.Volume;
            set => sampleChannel.Volume = value;
        }

        /// <summary>Reads from this wave stream</summary>
        /// <param name="buffer">Audio buffer</param>
        /// <param name="offset">Offset into buffer</param>
        /// <param name="count">Number of bytes required</param>
        /// <returns>Number of bytes read</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var waveBuffer = new WaveBuffer(buffer);
            var count1 = count / 4;
            return Read(waveBuffer.FloatBuffer, offset / 4, count1) * 4;
        }

        /// <summary>Reads audio from this sample provider</summary>
        /// <param name="buffer">Sample buffer</param>
        /// <param name="offset">Offset into sample buffer</param>
        /// <param name="count">Number of samples required</param>
        /// <returns>Number of samples read</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            lock (lockObject)
            {
                return sampleChannel.Read(buffer, offset, count);
            }
        }

        /// <summary>
        /// Disposes this AudioFileReader
        /// </summary>
        /// <param name="disposing">True if called from Dispose</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && readerStream != null)
            {
                readerStream.Dispose();
                readerStream = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates the reader stream, supporting all filetypes in the core NAudio library,
        /// and ensuring we are in PCM format
        /// </summary>
        /// <param name="fileName">The name of the file. Used only to determine the correct reader</param>
        /// <param name="stream">The stream to read from</param>
        private void CreateReaderStream(string fileName, Stream stream)
        {
            if (fileName.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {
                readerStream = new WaveFileReader(stream);
                if (readerStream.WaveFormat.Encoding is WaveFormatEncoding.Pcm or WaveFormatEncoding.IeeeFloat)
                {
                    return;
                }

                readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
                readerStream = new BlockAlignReductionStream(readerStream);
            }
            else if (fileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
            {
                readerStream = new Mp3FileReader(stream);
            }
            else if (fileName.EndsWith(".aiff", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".aif", StringComparison.OrdinalIgnoreCase))
            {
                readerStream = new AiffFileReader(stream);
            }
            else
            {
                // Sadly, the media foundation reader doesn't support reading from a stream
                throw new ArgumentException("The specified file type is not supported");
            }
        }

        /// <summary>
        /// Helper to convert source to dest bytes
        /// </summary>
        private long SourceToDest(long sourceBytes) => destBytesPerSample * (sourceBytes / sourceBytesPerSample);

        /// <summary>
        /// Helper to convert dest to source bytes
        /// </summary>
        private long DestToSource(long destBytes) => sourceBytesPerSample * (destBytes / destBytesPerSample);
    }
}
