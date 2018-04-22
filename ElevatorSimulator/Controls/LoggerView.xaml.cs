using ElevatorApp.Util;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            var viewer = VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(this.listViewEvents);
            if (viewer != null)
            {
                // User scroll event : set or unset auto-scroll mode
                if (e.ExtentHeightChange.Equals(0))

                {
                    // Content unchanged : user scroll event
                    if (viewer.VerticalOffset.Equals(viewer.ScrollableHeight))
                    {
                        // Scroll bar is in bottom Set auto-scroll mode
                        this._autoScroll = true;
                    }
                    else
                    {
                        // Scroll bar isn't in bottom Unset auto-scroll mode
                        this._autoScroll = false;
                    }
                }

                // Content scroll event : auto-scroll eventually
                if (this._autoScroll && !e.ExtentHeightChange.Equals(0))
                {
                    // Content changed and auto-scroll mode set Autoscroll
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
            //BindingOperations.EnableCollectionSynchronization(Logger.Events, _locker);
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnClear_click(object sender, RoutedEventArgs e)
        {
            this.listViewEvents.Items.Clear();
        }

        /// <summary>
        /// Handles the Loaded event of the StackPanel control.
        /// <para>This is when the <see cref="Logger"/> is wired up to write the events</para>
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Instance.ItemLogged -= this.ItemLogged;
            Logger.Instance.ItemLogged += this.ItemLogged;
        }

        /// <summary>
        /// Handler for <see cref="Logger.ItemLogged"/> that writes the event to the listbox of this view
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        private void ItemLogged(object sender, string message)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        this.listViewEvents.Items.Add(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            MessageBox.Show("Test dialog");

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var str = this.listViewEvents.Items.OfType<string>();

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