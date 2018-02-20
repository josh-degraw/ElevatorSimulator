using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElevatorApp.Core;
using ElevatorApp.GUI.ViewModels;

namespace ElevatorApp.GUI
{
    /// <summary>
    /// Interaction logic for ElevatorDisplay.xaml
    /// </summary>
    public partial class ElevatorDisplay : UserControl
    {
        public ElevatorViewModel ViewModel { get; } = new ElevatorViewModel();

        //private Border CreateBorder(int floor)
        //{
        //    var b = new Border
        //    {
        //        Padding = new Thickness(5, 15, 5, 15),
        //        Child = new WrapPanel
        //        {
        //            Children =
        //            {
        //                new Label { Content = floor },
        //                new Border
        //                {
        //                    Child=new Rectangle
        //                    {
        //                        Height = 37,
        //                        Width = 43,
        //                        Stroke = new SolidColorBrush { Color= Color.FromArgb(255,0,0,0) }
        //                    }
        //                }

        //            }
        //        }
        //    };
        //    return b;
        //}

        public ElevatorDisplay()
        {
            InitializeComponent();

            ((INotifyPropertyChanged)ViewModel).PropertyChanged += (a, b) =>
           {
               if (b.PropertyName == nameof(ViewModel.FloorCount))
               {

               }
           };
        }
    }
}
