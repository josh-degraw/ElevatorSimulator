using System.Collections.Generic;
using System.Linq;

namespace ElevatorApp.Util
{
    ///<inheritdoc/>
    public class IntStatistic : Statistic<int, double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntStatistic"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <inheritdoc />
        public IntStatistic(string name) : base(name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntStatistic"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collection"></param>
        /// <inheritdoc />
        public IntStatistic(string name, IEnumerable<int> collection) : base(name, collection)
        { }

        protected override int DefaultMin => int.MaxValue;

        ///<inheritdoc/>
        protected override int AddItems(int left, int right) => left + right;

        ///<inheritdoc/>
        protected override int DivideItems(int numerator, int denominator) => numerator / denominator;

        ///<inheritdoc/>
        protected override int MultiplyItems(int left, int right) => left * right;

        ///<inheritdoc/>
        protected override int SubtractItems(int left, int right) => left - right;

        ///<inheritdoc/>
        protected override double CalculateAverage() => this._collection.Average();
    }
}