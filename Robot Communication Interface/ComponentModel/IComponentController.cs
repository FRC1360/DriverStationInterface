using System.Threading.Tasks;
using System.Windows;

namespace Frc1360.DriverStation.RobotComm.ComponentModel
{
    public interface IComponentController
    {
        Task InitializeAsync(Connection connection);
        UIElement Display { get; }
    }
}
