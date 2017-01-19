using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Frc1360.DriverStation.RobotComm
{
    public abstract class ControllerBase
    {
        protected ControllerBase(Connection conn, byte channel, out Stream stream)
        {
            stream = conn.mcs.GetChannelStream(channel);
        }

        
    }
}
