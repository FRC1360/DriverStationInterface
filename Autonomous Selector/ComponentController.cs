using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.ComponentModel;

namespace Frc1360.DriverStation.Components.AutonomousSelector
{
    [Export(typeof(IComponentController))]
    [ExportMetadata("Name", "Autonomous Selector")]
    public sealed class ComponentController : IComponentController
    {
        UIElement IComponentController.Display => new Selector();

        async Task IComponentController.InitializeAsync(Connection connection)
        {
            throw new NotImplementedException();
        }
    }
}
