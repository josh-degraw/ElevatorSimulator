using System.Collections.Generic;
using System.Linq;

namespace ElevatorApp.Util
{
    /// <summary>
    /// Statistical values of type <see cref="decimal"/>
    /// </summary>
    /// <seealso cref="ElevatorApp.Util.SimpleStatistic{T}" />
    /// <inheritdoc />
    public class DecimalStatistic : SimpleStatistic<decimal>
    {
        ///<inheritdoc/>
        public DecimalStatistic(string name) : base(name)
        { }

        ///<inheritdoc/>
        public DecimalStatistic(string name, IEnumerable<decimal> collection) : base(name, collection)
        { }

        protected override decimal DefaultMin => decimal.MaxValue;

        ///<inheritdoc/>
        protected override decimal AddItems(decimal left, decimal right) => left + right;

        ///<inheritdoc/>
        protected override decimal DivideItems(decimal numerator, decimal denominator) => numerator / denominator;

        ///<inheritdoc/>
        protected override decimal MultiplyItems(decimal left, decimal right) => left * right;

        ///<inheritdoc/>
        protected override decimal SubtractItems(decimal left, decimal right) => left - right;

        ///<inheritdoc/>
        protected override decimal CalculateAverage() => this._collection.Average();
    }
}