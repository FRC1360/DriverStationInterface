using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Frc1360.DriverStation.RobotComm.Utilities;

namespace Frc1360.DriverStation.RobotComm
{
    public abstract class CommandControllerBase : ControllerBase
    {
        [ThreadStatic]
        private static Stream streamTemp;

        private Stream stream;
        private BinaryReader r;
        private BinaryWriter w;
        private Task rt;

        public CommandControllerBase(Connection conn, byte channel) : base(conn, channel, out streamTemp)
        {
            stream = streamTemp;
            r = new BinaryReader(stream);
            w = new BinaryWriter(stream);
            rt = ReceiveAsync();
        }

        private async Task ReceiveAsync()
        {
            while (true)
            {
                byte[] data = new byte[6];
                await stream.ReadAsync(data, 0, 6);
                ushort id = data.UInt16Big(0);
                int len = data.UInt32Big(2).Signed();
                data = new byte[len];
                await stream.ReadAsync(data, 0, len);
                HandleCommandAsync(id, data);
            }
        }

        private async void HandleCommandAsync(ushort id, byte[] data)
        {
            await Task.Run(() => OnCommand(id, data));
        }

        protected void SendCommand(ushort id, params object[] data)
        {
            lock (stream)
                using (var ms = new MemoryStream())
                using (var mw = new BinaryWriter(ms))
                {
                    foreach (var o in data)
                        if (Serialize(o) is byte[] r)
                            mw.Write(r);
                        else
                            throw new InvalidDataException($"Cannot serialize object of type '{o?.GetType().FullName ?? "null"}'");
                    w.WriteUInt16Big(id);
                    w.Write(data.Length.Unsigned().BigEndian());
                    ms.CopyTo(stream);
                }
        }

        protected virtual byte[] Serialize(object data)
        {
            switch (data)
            {
                case byte[] array:
                    return array;
                case byte b:
                    return new[] { b };
                case sbyte s:
                    return new[] { s.Unsigned() };
                case ushort u:
                    return u.BigEndian();
                case short s:
                    return s.Unsigned().BigEndian();
                case uint u:
                    return u.BigEndian();
                case int i:
                    return i.Unsigned().BigEndian();
                case ulong u:
                    return u.BigEndian();
                case long l:
                    return l.Unsigned().BigEndian();
                case string s:
                    return s.String1360();
                default:
                    return null;
            }
        }

        protected abstract void OnCommand(ushort id, byte[] data);
    }
}
