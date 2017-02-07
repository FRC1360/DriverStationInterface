using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Frc1360.DriverStation.RobotComm.Utilities;

namespace Frc1360.DriverStation.RobotComm
{
    public sealed class Connection
    {
        internal readonly MultiChannelStream mcs;

        public Connection(string hostname, int port, Action<Exception> handler)
        {
            var conn = new TcpClient();
            conn.Connect(hostname, port);
            var inter = new IntermediaryStream(conn.GetStream(), e =>
            {
                handler(e);
                conn.Connect(hostname, port);
                return conn.GetStream();
            });
            mcs = new MultiChannelStream(inter);
        }
    }
}
