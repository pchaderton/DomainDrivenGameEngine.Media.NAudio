using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DomainDrivenGameEngine.Media.Models;
using DomainDrivenGameEngine.Media.NAudio.IO;
using DomainDrivenGameEngine.Media.Readers;
using NAudio.Wave;

namespace DomainDrivenGameEngine.Media.NAudio.Readers
{
    /// <summary>
    /// An NAudio-based implementation of a <see cref="IMediaReader{SoundEffect}"/> for use with projects utilizing DomainDrivenGameEngine.Media.
    /// </summary>
    /// <remarks>
    /// Only supports formats that NAudio natively supports, minus any format loaded via the <see cref="MediaFoundationReader"/>.
    /// </remarks>
    public class NAudioSoundEffectReader : BaseMediaReader<SoundEffect>
    {
        /// <summary>
        /// The extensions this source service supports.
        /// </summary>
        private static readonly IReadOnlyCollection<string> SupportedExtensions = new string[]
        {
            ".wav",
            ".aiff",
            ".mp3",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="NAudioSoundEffectReader"/> class.
        /// </summary>
        public NAudioSoundEffectReader()
            : base(SupportedExtensions)
        {
        }

        /// <inheritdoc/>
        public override SoundEffect Read(Stream stream, string path, string extension)
        {
            using (var waveStream = GetWaveStream(stream, extension))
            {
                var waveProvider16 = waveStream.ToSampleProvider()
                                               .ToWaveProvider16();

                var allBytes = new List<byte[]>();
                var readBytes = 0;
                var totalReadBytes = 0;
                do
                {
                    var byteBuffer = new byte[1024];
                    readBytes = waveProvider16.Read(byteBuffer, 0, 1024);
                    if (readBytes > 0)
                    {
                        totalReadBytes += readBytes;
                        allBytes.Add(byteBuffer);
                    }
                }
                while (readBytes != 0);

                var bytes = allBytes.SelectMany(ba => ba)
                                    .Take(totalReadBytes)
                                    .ToArray();

                return new SoundEffect(waveProvider16.WaveFormat.Channels, waveProvider16.WaveFormat.SampleRate, new ReadOnlyCollection<byte>(bytes), stream);
            }
        }

        /// <summary>
        /// Gets the wave provider to use for loading the sound effect.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read the file with.</param>
        /// <param name="extension">The extension of the requested file.</param>
        /// <returns>A <see cref="IWaveProvider"/> to return.</returns>
        private WaveStream GetWaveStream(Stream stream, string extension)
        {
            switch (extension)
            {
                case ".wav":
                    return new WaveFileReader(stream);
                case ".mp3":
                    return new Mp3FileReader(stream);
                case ".aiff":
                    return new AiffFileReader(stream);
                default:
                    throw new Exception($"Unknown {nameof(extension)}: {extension}.");
            }
        }
    }
}
