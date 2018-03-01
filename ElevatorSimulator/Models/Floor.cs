using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Models
{
    public class Floor : ModelBase, IFloor, ISubcriber<ElevatorMasterController>
    {
        public int FloorNumber { get; } = 1;

        public ICollection<FloorButton> FloorButtons { get; private set; }

        private ElevatorCallPanel _callPanel = new ElevatorCallPanel(1);
        public ElevatorCallPanel CallPanel
        {
            get => _callPanel;
            set => SetProperty(ref _callPanel, value);
        }


        public Floor(int floorNumber, ElevatorCallPanel callPanel)
        {
            this.FloorNumber = floorNumber;
            this.CallPanel = callPanel;
        }

        public Floor(int floorNumber) : this(floorNumber, new ElevatorCallPanel(floorNumber))
        {

        }
        
        public Floor()
        {

        }

        private bool _subscribed = false;

        bool ISubcriber<ElevatorMasterController>.Subscribed => _subscribed;

        public void Subscribe(ElevatorMasterController parent)
        {
            //if (Subscribed)
            //    return;
            this.CallPanel.Subscribe(parent);
            this._subscribed = true;
        }
    }
}
