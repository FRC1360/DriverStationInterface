using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frc1360.DriverStation.RobotComm.Utilities
{
    public sealed class IntermediaryStream : Stream
    {
        Stream baseStream;
        Func<Exception, Stream> handler;

        public IntermediaryStream(Stream baseStream, Func<Exception, Stream> handler)
        {
            this.baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        private void Try(Action action)
        {
            lock (this)
                while (true)
                {
                    try
                    {
                        action();
                        return;
                    }
                    catch (Exception e)
                    {
                        baseStream = handler(e);
                    }
                }
        }

        private T Try<T>(Func<T> action)
        {
            lock (this)
                while (true)
                {
                    try
                    {
                        return action();
                    }
                    catch (Exception e)
                    {
                        baseStream = handler(e);
                    }
                }
        }

        public override bool CanRead => Try(() => baseStream.CanRead);

        public override bool CanSeek => Try(() => baseStream.CanSeek);

        public override bool CanWrite => Try(() => baseStream.CanWrite);

        public override long Length => Try(() => baseStream.Length);

        public override long Position { get => Try(() => baseStream.Position); set => Try(() => baseStream.Position = value); }

        public override void Flush() => Try(baseStream.Flush);

        public override int Read(byte[] buffer, int offset, int count) => Try(() => baseStream.Read(buffer, offset, count));

        public override long Seek(long offset, SeekOrigin origin) => Try(() => baseStream.Seek(offset, origin));

        public override void SetLength(long value) => Try(() => baseStream.SetLength(value));

        public override void Write(byte[] buffer, int offset, int count) => Try(() => baseStream.Write(buffer, offset, count));
    }
}
