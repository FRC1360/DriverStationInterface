using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Frc1360.DriverStation.RobotComm;
using Frc1360.DriverStation.RobotComm.Utilities;

namespace Frc1360.DriverStation.Components.AutonomousSelector
{
    public sealed class AutonController : CommandControllerBase
    {
        private ObservableCollection<AutonRoutine> options = new ObservableCollection<AutonRoutine>();

        public AutonController(Connection conn) : base(conn, 0)
        {
            SendCommand(0);
            AutonomousSelection = new ObservableCollection<AutonRoutine>();
            AutonomousSelection.CollectionChanged += AutonomousSelectionChanged;
        }

        private void AutonomousSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SendCommand(1, (from i in e.NewItems.OfType<AutonRoutine>() select i.ID as object).ToArray());
                    break;
            }
        }

        public ObservableCollection<AutonRoutine> AutonomousSelection { get; private set; }

        public ReadOnlyObservableCollection<AutonRoutine> AutonomousOptions => new ReadOnlyObservableCollection<AutonRoutine>(options);

        protected override void OnCommand(ushort id, byte[] data)
        {
            switch (id)
            {
                case 0:
                    options.Add(new AutonRoutine(Encoding.UTF8.GetString(data, 2, data.Length - 2), data.UInt16Big(0)));
                    break;
                case 1:
                    options.Clear();
                    break;
            }
        }
    }
}
