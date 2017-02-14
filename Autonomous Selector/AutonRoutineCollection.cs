using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Frc1360.DriverStation.Components.AutonomousSelector
{
    public sealed class AutonRoutineCollection : IReadOnlyCollection<AutonRoutineCollection.Container>
    {
        private Container[] containers;
        private List<UpdateHandler> handlers = new List<UpdateHandler>();

        public AutonRoutineCollection(int size)
        {
            containers = new Container[size];
            for (int i = 0; i < size; ++i)
                containers[i] = new Container(this, i, null);
        }

        int IReadOnlyCollection<Container>.Count => containers.Length;

        IEnumerator IEnumerable.GetEnumerator() => containers.GetEnumerator();

        IEnumerator<Container> IEnumerable<Container>.GetEnumerator() => ((IEnumerable<Container>)containers).GetEnumerator();

        public Container this[int index] => containers[index];

        public event UpdateHandler Updated
        {
            add
            {
                handlers.Add(value);
            }
            remove
            {
                handlers.Remove(value);
            }
        }

        public delegate void UpdateHandler(int index, AutonRoutine routine);

        public sealed class Container
        {
            private AutonRoutineCollection c;
            private int n;
            private AutonRoutine r;

            internal Container(AutonRoutineCollection c, int n, AutonRoutine r)
            {
                this.c = c;
                this.n = n;
                this.r = r;
            }

            public AutonRoutine Value
            {
                get
                {
                    return r;
                }
                set
                {
                    r = value;
                    foreach (var h in c.handlers)
                        h.Invoke(n, value);
                }
            }

            public ObservableCollection<AutonRoutine> Options { get; } = new ObservableCollection<AutonRoutine>();
        }
    }
}
