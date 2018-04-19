using System.Collections.Generic;
using System.Linq;

namespace ElevatorApp.Util
{
    ///<inheritdoc/>
    public class DoubleStatistic : SimpleStatistic<double>
    {
        ///<inheritdoc/>
        public DoubleStatistic(string name) : base(name)
        { }

        ///<inheritdoc/>
        public DoubleStatistic(string name, IEnumerable<double> collection) : base(name, collection)
        { }

        ///<inheritdoc/>
        protected override double AddItems(double left, double right) => left + right;

        ///<inheritdoc/>
        protected override double DivideItems(double numerator, double denominator) => numerator / denominator;

        ///<inheritdoc/>
        protected override double MultiplyItems(double left, double right) => left * right;

        ///<inheritdoc/>
        protected override double SubtractItems(double left, double right) => left - right;

        ///<inheritdoc/>
        protected override double CalculateAverage() => this._collection.Average();
    }
}