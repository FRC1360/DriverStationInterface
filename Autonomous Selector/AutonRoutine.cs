using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frc1360.DriverStation.Components.AutonomousSelector
{
    public sealed class AutonRoutine
    {
        private string name;
        private ushort id;

        internal AutonRoutine(string name, ushort id)
        {
            this.name = name;
            this.id = id;
        }

        public string Name => name;

        public ushort ID => id;
    }
}
