using System;
using System.Collections.Generic;
using System.Globalization;
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
using ElevatorApp.Util;
using Duration = NodaTime.Duration;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for StatisticControl.xaml
    /// </summary>
    public partial class StatisticControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticControl"/> class.
        /// </summary>
        public StatisticControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// <see cref="Stats.Instance"/>
        /// </value>
        public Stats ViewModel => Stats.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// A class used to convert a value of type <see cref="T:NodaTime.Duration" /> to a string in XAML
    /// </summary>
    /// <seealso cref="T:System.Windows.Data.IValueConverter" />
    public class DurationConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        /// <summary>Converts a value. </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const string format = "M:ss.ff";
            if (value is Duration dur)
            {
                if (dur.Equals(Duration.Zero) ||dur.Equals(Duration.MaxValue) || dur.Equals(Duration.MinValue))
                    return "-";

                return dur.ToString(format, null);
            }

            return default(Duration).ToString(format, null);
            
        }

        /// <inheritdoc />
        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && TimeSpan.TryParse(str, out TimeSpan tSpan))
            {
                return Duration.FromTimeSpan(tSpan);
            }

            return default(Duration);
        }

        #endregion
    }
}
