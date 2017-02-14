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
