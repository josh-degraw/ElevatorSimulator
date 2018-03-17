using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ElevatorApp.Util;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core.Utilities;


namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for LoggerView.xaml
    /// </summary>
    public partial class LoggerView : UserControl
    {
        private bool _autoScroll = true;

        /// <summary>
        /// Scroll to the bottom when a new item has been logged, if it was previously scrolled all the way down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var viewer = VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(_scrollViewer);
            if (viewer != null)
            { 
                // User scroll event : set or unset auto-scroll mode
                if (e.ExtentHeightChange.Equals(0))

                {
                    // Content unchanged : user scroll event
                    if (viewer.VerticalOffset.Equals(viewer.ScrollableHeight))
                    {
                        // Scroll bar is in bottom
                        // Set auto-scroll mode
                        _autoScroll = true;
                    }
                    else
                    {
                        // Scroll bar isn't in bottom
                        // Unset auto-scroll mode
                        _autoScroll = false;
                    }
                }

                // Content scroll event : auto-scroll eventually
                if (_autoScroll && !e.ExtentHeightChange.Equals(0))
                {
                    // Content changed and auto-scroll mode set
                    // Autoscroll
                    viewer.ScrollToVerticalOffset(viewer.ExtentHeight);
                }

            }

        }

        private readonly object _locker = new object();
        public LoggerView()
        {
            BindingOperations.EnableCollectionSynchronization(Logger.Events, _locker);
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Logger.Instance.ClearItems();
        }

    }
}
