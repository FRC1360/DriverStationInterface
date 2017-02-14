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
        private byte[] data;

        public ConfigController(Connection conn, byte channel) : base(conn, channel)
        {
            SendCommand(0);
            ewh.WaitOne();
        }

        public IEnumerable Options
        {
            get
            {
                ushort count = data.UInt16Big(0);
                int off = 2;
                for (int i = 0; i < count; ++i)
                {
                    var s = data.String1360(off);
                    yield return s;
                    off += s.Length;
                }
            }
        }

        public void Update(int selection) => SendCommand(1, selection);

        protected override void OnCommand(ushort id, byte[] data)
        {
            switch (id)
            {
                case 0:
                    this.data = data;
                    ewh.Set();
                    break;
            }
        }
    }
}
