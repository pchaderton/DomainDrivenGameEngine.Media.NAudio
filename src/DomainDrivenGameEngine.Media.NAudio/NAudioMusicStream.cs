using System;
using System.IO;
using NAudio.Wave;

namespace DomainDrivenGameEngine.Media.NAudio
{
    /// <summary>
    /// A <see cref="Stream"/> for reading music data from an NAudio <see cref="WaveStream"/>.
    /// </summary>
    internal class NAudioMusicStream : Stream
    {
        /// <summary>
        /// The <see cref="WaveStream"/> to use for streaming data.
        /// </summary>
        private readonly WaveStream _stream;

        /// <summary>
        /// The <see cref="IWaveProvider"/> which provides decoded data to the caller.
        /// </summary>
        private readonly IWaveProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="NAudioMusicStream"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="WaveStream"/> to use for streaming data.</param>
        /// <param name="provider">The <see cref="IWaveProvider"/> which provides decoded data to the caller.</param>
        public NAudioMusicStream(WaveStream stream, IWaveProvider provider)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override long Length => _stream.Length;

        /// <inheritdoc/>
        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        /// <summary>
        /// Disposes of this stream.
        /// </summary>
        public new void Dispose()
        {
            _stream.Dispose();
            base.Dispose();
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _provider.Read(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
