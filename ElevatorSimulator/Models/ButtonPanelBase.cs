using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    public class ButtonPanelBase : ICollection<FloorButton>, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        public ICollection<FloorButton> FloorButtons { get; private set; }

        public ButtonPanelBase()
        {
            Logger.LogEvent($"Initializing ButtonPanel");
            this.FloorButtons = new ObservableCollection<FloorButton>
            {
                new FloorButton(4),
                new FloorButton(3),
                new FloorButton(2),
                new FloorButton(1),
            };
            Logger.LogEvent("Done initializing ButtonPanel");
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

        public bool Subscribed { get; protected set; }



        public virtual void Subscribe((ElevatorMasterController, Elevator) parent)
        {
            foreach (FloorButton button in this.FloorButtons)
            {
                button.Subscribe(parent);
            }
        }
    }
}
