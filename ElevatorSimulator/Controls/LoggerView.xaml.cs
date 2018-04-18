using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;
using MoreLinq;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core.Utilities;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;


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
            var viewer = VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(listViewEvents);
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
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerView"/> class.
        /// </summary>
        public LoggerView()
        {
            BindingOperations.EnableCollectionSynchronization(Logger.Events, _locker);
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listViewEvents.Items.Clear();
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Instance.ItemLogged -= ItemLogged;
            Logger.Instance.ItemLogged += ItemLogged;
        }

        private void ItemLogged(object sender, string message)
        {
            try
            {
                Dispatcher.Invoke(() => listViewEvents.Items.Add(message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            MessageBox.Show("Test dialog"); 

            if (dialog.ShowDialog() ==true)
            {
                try
                {
                    var str = listViewEvents.Items.OfType<string>();

                    File.WriteAllLines(dialog.FileName, str);
                    MessageBox.Show("Saved file at " + dialog.SafeFileName, "Success", MessageBoxButton.OK);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error saving file", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //File.WriteAllText(dialog.FileName, txtEditor.Text);
            }
        }
    }
}
