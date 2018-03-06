using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Models
{
    public class Floor : ModelBase, IFloor, ISubcriber<ElevatorMasterController>, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        private int _floorNum = 1;

        public int FloorNumber
        {
            get => _floorNum;
            private set => SetProperty(ref _floorNum, value);

        }

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

        private bool _subscribed = false, _controllerSubscribed = false;

        bool ISubcriber<(ElevatorMasterController, Elevator)>.Subscribed => _subscribed;
        bool ISubcriber<ElevatorMasterController>.Subscribed => _controllerSubscribed;

        async Task ISubcriber<ElevatorMasterController>.Subscribe(ElevatorMasterController parent)
        {
            if (_controllerSubscribed)
                return;

            await this.CallPanel.Subscribe(parent).ConfigureAwait(false);

            this._controllerSubscribed = true;
        }

        public async Task Subscribe((ElevatorMasterController, Elevator) parents)
        {
            if (this._subscribed)
                return;

            await this.CallPanel.Subscribe(parents).ConfigureAwait(false);
            var (_, elevator) = parents;
            FloorButton thisFloorButton = this.CallPanel.SingleOrDefault(a => a.FloorNum == this.FloorNumber);

            if (thisFloorButton != null)
            {
                elevator.OnDeparture += (e, floor) =>
                {
                    thisFloorButton.Disable();
                };

                elevator.OnArrival += (e, floor) =>
                {
                    thisFloorButton.Disable();
                };
                
            }

            this._subscribed = true;
        }


    }
}
