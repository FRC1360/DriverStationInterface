using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Frc1360.DriverStation.RobotComm.Utilities
{
    public sealed class IntermediaryStream : Stream
    {
        Stream baseStream;
        Func<Exception, Stream> handler;
        object rl = new object(), wl = new object(), ml = new object();
        EventWaitHandle ewh = new EventWaitHandle(true, EventResetMode.ManualReset);
        HashSet<Thread> currentThreads = new HashSet<Thread>();

        public IntermediaryStream(Stream baseStream, Func<Exception, Stream> handler)
        {
            this.baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        private void Try(object _lock, Action action)
        {
            while (true)
                try
                {
                    lock (currentThreads)
                        currentThreads.Add(Thread.CurrentThread);
                    ewh.WaitOne();
                    lock (_lock)
                        try
                        {
                            action();
                            return;
                        }
                        catch (Exception e)
                        {
                            ewh.Reset();
                            lock (currentThreads)
                            {
                                foreach (var t in currentThreads)
                                    t.Interrupt();
                                currentThreads.Clear();
                            }
                            Thread.Yield();
                            while (true)
                                try
                                {
                                    baseStream = handler(e);
                                    break;
                                }
                                catch
                                {
                                    continue;
                                }
                            ewh.Set();
                        }
                }
                catch (ThreadInterruptedException)
                {
                    continue;
                }
                finally
                {
                    lock (currentThreads)
                        if (currentThreads.Contains(Thread.CurrentThread))
                            currentThreads.Remove(Thread.CurrentThread);
                }
        }

        private T Try<T>(object _lock, Func<T> action)
        {
            object result = null;
            Try(_lock, new Action(() => result = action()));
            return (T)result;
        }

        public override bool CanRead => Try(ml, () => baseStream.CanRead);

        public override bool CanSeek => Try(ml, () => baseStream.CanSeek);

        public override bool CanWrite => Try(ml, () => baseStream.CanWrite);

        public override long Length => Try(ml, () => baseStream.Length);

        public override long Position
        {
            get => Try(ml, () => baseStream.Position);
            set => Try(ml, () => baseStream.Position = value);
        }

        public override void Flush() => Try(wl, baseStream.Flush);

        public override int Read(byte[] buffer, int offset, int count) => Try(rl, () => baseStream.Read(buffer, offset, count));

        public override long Seek(long offset, SeekOrigin origin) => Try(ml, () => baseStream.Seek(offset, origin));

        public override void SetLength(long value) => Try(ml, () => baseStream.SetLength(value));

        public override void Write(byte[] buffer, int offset, int count) => Try(wl, () => baseStream.Write(buffer, offset, count));

        protected override void Dispose(bool disposing) => ewh.Dispose();
    }
}
