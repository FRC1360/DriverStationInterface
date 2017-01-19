using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Frc1360.DriverStation.RobotComm.ComponentModel;
using Frc1360.DriverStation.Properties;

namespace Frc1360.DriverStation
{
    public sealed class Components
    {
        private static Components instance;

        public IEnumerable<Lazy<IComponentController, IComponentControllerMetadata>> Controllers;

        private Components()
        {
            new CompositionContainer(new DirectoryCatalog(Environment.ExpandEnvironmentVariables(Settings.Default.ComponentsDirectory))).ComposeParts(this);
        }

        static Components()
        {
            instance = new Components();
        }

        public static IEnumerable<Lazy<IComponentController, IComponentControllerMetadata>> ComponentControllers => instance.Controllers;
    }
}
