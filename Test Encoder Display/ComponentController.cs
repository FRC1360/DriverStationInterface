using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.ComponentModel;

namespace Frc1360.DriverStation.Components.TestEncoderDisplay
{
    [Export(typeof(IComponentController))]
    [ExportMetadata("Name", "Test Encoder Display")]
    public class ComponentController : IComponentController
    {
        private Display display;

        private EncoderController controller;

        public UIElement Display => display;

        public Task InitializeAsync(Connection connection) => Task.Run(() =>
        {
            Application.Current.Dispatcher.Invoke(() => display = new Display());
            controller = new EncoderController(connection, display);
        });
    }
}
