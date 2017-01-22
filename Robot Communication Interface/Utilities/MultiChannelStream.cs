using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Frc1360.DriverStation.RobotComm.Utilities
{
    public sealed class MultiChannelStream : IDisposable
    {
        Stream stream;
        MemoryStream[] buffers = new MemoryStream[256];
        Action[] notifiers = new Action[256];
        Action errorNotifiers = null;
        ChannelStream[] streams = new ChannelStream[256];
        object rl = new object(), wl = new object();
        bool work = true;
        Thread rt;

        public MultiChannelStream(Stream s)
        {
            stream = s;
            rt = new Thread(() =>
              {
                  try
                  {
                      using (var r = new BinaryReader(stream))
                          while (work)
                          {
                              byte c;
                              ushort l;
                              byte[] data;
                              lock (rl)
                              {
                                  c = r.ReadByte();
                                  l = r.ReadUInt16Big();
                                  data = r.ReadBytes(l);
                              }
                              lock (buffers[c])
                              {
                                  (buffers[c] ?? (buffers[c] = new MemoryStream())).Write(data, 0, l);
                                  notifiers[c]?.Invoke();
                              }
                          }
                  }
                  catch { errorNotifiers?.Invoke(); }
              });
            rt.Start();
            Thread.Yield();
        }

        public Stream GetChannelStream(byte channel) => streams[channel] ?? (streams[channel] = new ChannelStream(this, channel));

        public void Dispose()
        {
            try
            {
                lock (wl)
                    lock (rl)
                    {
                        stream?.Dispose();
                        for (int i = 0; i < 256; ++i)
                            buffers[i]?.Dispose();
                        work = false;
                        rt.Interrupt();
                    }
            }
            catch { }
        }

        private sealed class ChannelStream : Stream
        {
            MultiChannelStream mcs;
            byte ch;
            long pos;

            public ChannelStream(MultiChannelStream stream, byte channel)
            {
                mcs = stream;
                ch = channel;
                lock (mcs.buffers[ch] ?? (mcs.buffers[ch] = new MemoryStream()))
                {
                    if (mcs.buffers[ch] == null)
                        mcs.buffers[ch] = new MemoryStream();
                }
            }

            public override bool CanRead { get; } = true;

            public override bool CanSeek { get; } = false;

            public override bool CanWrite { get; } = true;

            public override long Length
            {
                get
                {
                    long len = 0;
                    lock (mcs.buffers[ch])
                        len = mcs.buffers[ch].Length;
                    return len;
                }
            }

            public override long Position
            {
                get
                {
                    return pos;
                }

                set
                {
                    throw new NotSupportedException();
                }
            }

            public override void Flush()
            {
                lock (mcs.wl)
                    mcs.stream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                int r = 0;
                bool good = true;
                bool error = false;
                Action read = () =>
                {
                    var buf = mcs.buffers[ch].GetBuffer();
                    int nb = count - r;
                    while (r < count && pos < mcs.buffers[ch].Length)
                        buffer[offset + r++] = buf[pos++];
                };
                Action ecb = () =>
                    {
                        error = true;
                        good = true;
                    };
                Action cb = null;
                cb = () =>
                {
                    read();
                    if (r < count)
                        return;
                    mcs.notifiers[ch] -= cb;
                    mcs.errorNotifiers -= ecb;
                    good = true;
                };
                lock (mcs.buffers[ch])
                {
                    read();
                    if (r >= count)
                        goto done;
                    mcs.notifiers[ch] += cb;
                    mcs.errorNotifiers += ecb;
                    good = false;
                }
            done:
                while (!good)
                    Thread.Sleep(10);
                if (error)
                    throw new IOException("Failed to read from underlying stream.");
                return r;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                while (count != 0)
                {
                    int n = Math.Min(count, ushort.MaxValue);
                    lock (mcs.wl)
                    {
                        using (var w = new BinaryWriter(mcs.stream, Encoding.UTF8, true))
                        {
                            w.Write(ch);
                            w.WriteUInt16Big((ushort)n);
                            w.Flush();
                        }
                        mcs.stream.Write(buffer, offset, n);
                        mcs.stream.Flush();
                    }
                    offset += n;
                    count -= n;
                }
            }
        }
    }
}