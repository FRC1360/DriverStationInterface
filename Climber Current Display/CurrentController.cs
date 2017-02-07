using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.Utilities;

namespace Frc1360.DriverStation.Components.ClimberCurrent
{
    public sealed class CurrentController : ControllerBase
    {
        private static Stream s;
        private Display d;
        private ObservableCollection<float> data = new ObservableCollection<float>();

        public CurrentController(Connection conn, Display display) : base(conn, 1, out s)
        {
            d = display;
            Task.Run(() =>
            {
                using (var r = new BinaryReader(s))
                    while (true)
                    {
                        var c = r.ReadFloat1360();
                        d.Dispatcher.Invoke(() =>
                        {
                            d.current.Text = $"{c} A";
                            data.Add(c);
                        });
                    }
            });
        }
    }
}
