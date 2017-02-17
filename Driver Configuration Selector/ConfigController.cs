using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.Utilities;

namespace Frc1360.DriverStation.Components.ConfigSelector
{
    public sealed class ConfigController : CommandControllerBase
    {
        private EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.AutoReset);

        public ConfigController(Connection conn, byte channel) : base(conn, channel)
        {
            SendCommand(0);
            ewh.WaitOne();
        }

        public IEnumerable Options { get; private set; }

        public int Index { get; private set; }

        public void Update(int selection) => SendCommand(1, selection);

        protected override void OnCommand(ushort id, byte[] data)
        {
            switch (id)
            {
                case 0:
                    IEnumerable generate()
                    {
                        int count = data.UInt32Big(4).Signed();
                        int off = 8;
                        for (int i = 0; i < count; ++i)
                        {
                            var s = data.String1360(off);
                            yield return s;
                            off += s.Length + 4;
                        }
                    }
                    Index = data.UInt32Big(0).Signed();
                    Options = generate();
                    ewh.Set();
                    break;
            }
        }
    }
}
