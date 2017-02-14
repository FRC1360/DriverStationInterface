using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Frc1360.DriverStation.RobotComm.ComponentModel;

namespace Frc1360.DriverStation
{
    public sealed class Components
    {
        private static Components instance;

        [ImportMany]
        public IEnumerable<Lazy<IComponentController, IComponentControllerMetadata>> Controllers;

        private Components()
        {
            if (!Directory.Exists(App.ComponentsDirectory))
                Directory.CreateDirectory(App.ComponentsDirectory);
            new CompositionContainer(new DirectoryCatalog(App.ComponentsDirectory)).ComposeParts(this);
        }

        static Components()
        {
            instance = new Components();
        }

        public static IEnumerable<Lazy<IComponentController, IComponentControllerMetadata>> ComponentControllers => instance.Controllers;
    }
}
