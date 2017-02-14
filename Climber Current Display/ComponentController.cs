using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel.Composition;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.ComponentModel;

namespace Frc1360.DriverStation.Components.ClimberCurrent
{
    [Export(typeof(IComponentController))]
    [ExportMetadata("Name", "Climber Current Display")]
    public sealed class ComponentController : IComponentController
    {
        private Display display;

        public CurrentController Controller;

        public UIElement Display => display;

        public Task InitializeAsync(Connection connection) => Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() => display = new Display());
                Controller = new CurrentController(connection, display);
            });
    }
}
