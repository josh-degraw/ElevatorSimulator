using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    public class ButtonPanel : ICollection<FloorButton>, IButtonPanel, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        public ICollection<FloorButton> FloorButtons { get; private set; }

        public DoorButton OpenDoorButton { get; }
        public DoorButton CloseDoorButton { get; }

        public ButtonPanel()
        {
            Logger.LogEvent($"Initializing ButtonPanel {this.GetHashCode()}");
            this.OpenDoorButton = DoorButton.Open();
            this.CloseDoorButton = DoorButton.Close();
            this.FloorButtons = new ObservableCollection<FloorButton>
            {
                new FloorButton(4),
                new FloorButton(3, active: true),
                new FloorButton(2),
                new FloorButton(1),
            };
            Logger.LogEvent($"Done initializing ButtonPanel {this.GetHashCode()}");

        }

        #region ICollectionImplementation
        public IEnumerator<FloorButton> GetEnumerator()
        {
            return FloorButtons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)FloorButtons).GetEnumerator();
        }

        public void Add(FloorButton item)
        {
            FloorButtons.Add(item);
        }

        public void Clear()
        {
            FloorButtons.Clear();
        }

        public bool Contains(FloorButton item)
        {
            return FloorButtons.Contains(item);
        }

        void ICollection<FloorButton>.CopyTo(FloorButton[] array, int arrayIndex)
        {
            FloorButtons.CopyTo(array, arrayIndex);
        }

        public bool Remove(FloorButton item)
        {
            return FloorButtons.Remove(item);
        }

        public int Count => FloorButtons.Count;

        public bool IsReadOnly => FloorButtons.IsReadOnly;
        #endregion

        public bool Subscribed { get; private set; }

        public void Subscribe((ElevatorMasterController, Elevator) parent)
        {
            //if (Subscribed)
            //    return;

            var (controller, elevator) = parent;

            foreach (FloorButton button in this.FloorButtons)
            {
                button.Subscribe(parent);
            }

            controller.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(controller.FloorCount))
                {
                    this.FloorButtons.Clear();

                    // Range from 1 to count, reversed so it goes low to high vertically
                    foreach (int i in Enumerable.Range(1, controller.FloorCount).Reverse())
                    {
                        this.FloorButtons.Add(new FloorButton(i));
                    }

                }
            };

            elevator.Door.Subscribe(elevator);

            this.Subscribed = true;
        }

    }
}
