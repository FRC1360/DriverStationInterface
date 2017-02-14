using System.ComponentModel.Composition;
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
        private Selector selector;

        public AutonController Controller;

        public UIElement Display => selector;

        public Task InitializeAsync(Connection connection) => Task.Run(() =>
            {
                Controller = new AutonController(connection);
                Application.Current.Dispatcher.Invoke(() => selector = new Selector(Controller));
            });
    }
}
