using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.ComponentModel;

namespace Frc1360.DriverStation.Components.ConfigSelector
{
    [Export(typeof(IComponentController))]
    [ExportMetadata("Name", "Driver Configuration Selector")]
    public sealed class ComponentController : IComponentController
    {
        private Selector selector;

        public ConfigController Controller;

        public UIElement Display => selector;

        public Task InitializeAsync(Connection connection) => Task.Run(() =>
        {
            Controller = new ConfigController(connection, 3);
        });
    }
}
