using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.Utilities;

namespace Frc1360.DriverStation.Components.TestEncoderDisplay
{
    public class EncoderController : ControllerBase
    {
        [ThreadStatic]
        private static Stream tempStream;

        private Stream stream;

        public EncoderController(Connection conn, Display display) : base(conn, 2, out tempStream)
        {
            stream = tempStream;
            Task.Run(() =>
            {
                using (var r = new BinaryReader(stream))
                    while (true)
                    {
                        
                    }
            });
        }
    }
}
