using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Frc1360.DriverStation.RobotComm.Utilities;

namespace Frc1360.DriverStation.RobotComm
{
    public sealed class Connection
    {
        private TcpClient conn;
        internal readonly MultiChannelStream mcs;

        public Connection(IPEndPoint target)
        {
            conn = new TcpClient();
            conn.Connect(target);
            mcs = new MultiChannelStream(conn.GetStream());
        }
    }
}
