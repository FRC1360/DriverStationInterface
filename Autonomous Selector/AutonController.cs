using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.Utilities;

namespace Frc1360.DriverStation.Components.AutonomousSelector
{
    public sealed class AutonController : CommandControllerBase
    {
        private EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.ManualReset);

        public AutonController(Connection conn) : base(conn, 0)
        {
            SendCommand(0);
            ewh.WaitOne();
        }

        public AutonRoutineCollection AutonomousSelection { get; private set; }

        protected override void OnCommand(ushort id, byte[] data)
        {
            lock (this)
                switch (id)
                {
                    case 0:
                        if (ewh != null)
                        {
                            var len = data.UInt16Big(0);
                            AutonomousSelection = new AutonRoutineCollection(len);
                            AutonomousSelection.Updated += AutonomousSelectionUpdated;
                            ewh.Set();
                        }
                        break;
                    case 1:
                        AutonomousSelection[data.UInt16Big(0)].Options.Add(new AutonRoutine(Encoding.UTF8.GetString(data, 4, data.Length - 4), data.UInt16Big(2)));
                        break;
                }
        }

        private void AutonomousSelectionUpdated(int index, AutonRoutine routine) => SendCommand(1, index, routine.ID);
    }
}
