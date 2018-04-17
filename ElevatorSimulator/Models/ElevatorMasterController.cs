using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Properties;
using ElevatorApp.Util;
using MoreLinq;

namespace ElevatorApp.Models
{

    /// <summary>
    /// Represents the master controller for the system. Contains the Elevator and the floors
    /// </summary>
    public class ElevatorMasterController : ModelBase
    {
        #region Backing fields
        private int _floorHeight;

        private readonly Elevator _elevator = new Elevator();

        private readonly AsyncObservableCollection<Floor> _floors = new AsyncObservableCollection<Floor>(Enumerable.Range(1, 4).Reverse().Select(a => new Floor(a)));
        //  private readonly AsyncObservableCollection<int> _floorsRequested = new AsyncObservableCollection<int>();

        #endregion

        #region Properties
        /// <summary>
        /// A Read-only collection of <see cref="Models.Elevator"/> objects that will be managed by this <see cref="ElevatorMasterController"/>
        /// </summary>
        public Elevator Elevator => _elevator;

        /// <summary>
        /// <para>A Read-only collection of <see cref="Floor"/> objects that will be managed by this <see cref="ElevatorMasterController"/>.</para>
        /// <para>These represent the floors of the building.</para>
        /// </summary>
        public IReadOnlyCollection<Floor> Floors => _floors;

        /// <summary>
        /// Represents the height of the floors in feet
        /// </summary>
        public int FloorHeight
        {
            get => _floorHeight;
            set => SetProperty(ref _floorHeight, value);
        }

        public ElevatorSettings ElevatorSettings { get; } = new ElevatorSettings();


        /// <summary>
        /// Adjusts a private collection of the <see cref="ElevatorMasterController"/> based on the number of elements the collection is supposed to contain
        /// </summary>
        private void AdjustCollection<T>(ICollection<T> collection, int value, Func<int, T> generator, [CallerMemberName] string memberName = null) where T : ISubcriber<ElevatorMasterController>
        {
            if (collection.Count == value)
                return;

            if (value > collection.Count)
            {
                for (int i = collection.Count; i < value; i++)
                {
                    T newItem = generator(i);
                    newItem.Subscribe(this);
                    collection.Add(newItem);
                }
            }
            else
            {
                for (int i = collection.Count; i > value && i > 0; i--)
                {
                    T item = collection.LastOrDefault();

                    if (!Equals(item, default)) // If LastOrDefault returned default, 
                    {                           // the collection is empty, so don't do anything here
                        collection.Remove(item);
                    }
                }
            }
            this.OnPropertyChanged(collection.Count, memberName);
        }

        /// <summary>
        /// The number of floors currently being tracked.
        /// <para>
        /// <warning>Changing this number will change the number of elements in <see cref="Floors"/></warning>
        /// </para> 
        /// </summary>
        public int FloorCount
        {
            get => Floors.Count;
            set => AdjustCollection(_floors, value, floorNum => new Floor(floorNum));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Runs the necessary functions <see cref="Models.Elevator"/> respond appropriately to this <see cref="ElevatorMasterController"/>
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            Logger.LogEvent($"Initializing {nameof(ElevatorMasterController)}");

            await this.Elevator.Subscribe(this);

            await Task.WhenAll(this.Floors.Select(floor => floor.Subscribe(this)));
            Logger.LogEvent($"Initialized {nameof(ElevatorMasterController)}");
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="ElevatorMasterController"/>
        /// </summary>
        public ElevatorMasterController()
        {
        }


    }

}
