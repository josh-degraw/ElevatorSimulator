using System;
using System.Windows;
using System.Windows.Controls;
using ElevatorApp.Models;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for DoorControl.xaml
    /// </summary>
    public partial class DoorControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoorControl"/> class.
        /// </summary>
        public DoorControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Half of the time it takes to change from open to closed, or vice versia
        /// </summary>
        public static Duration TransitionTimeHalf { get; } = new Duration(TimeSpan.FromSeconds(Door.TRANSITION_TIME_SECONDS / 2d));

        /// <summary>
        /// Gets the time spent open.
        /// </summary>
        public Duration TimeSpentOpen => new Duration(TimeSpan.FromSeconds(Door.TIME_SPENT_OPEN_SECONDS));

    }

}