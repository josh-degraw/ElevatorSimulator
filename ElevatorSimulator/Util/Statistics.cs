using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace ElevatorApp.Util
{
    ///<inheritdoc/>
    public class DurationStatistic : SimpleStatistic<Duration>
    {
        ///<inheritdoc/>
        public DurationStatistic(string name) : base(name)
        { }

        ///<inheritdoc/>
        public DurationStatistic(string name, IEnumerable<Duration> collection) : base(name, collection)
        { }
        ///<inheritdoc/>
        protected override Duration AddItems(Duration left, Duration right) => left + right;

        ///<inheritdoc/>
        protected override Duration CalculateAverage() => Duration.FromNanoseconds(_collection.Average(d => d.TotalNanoseconds));

        ///<inheritdoc/>
        protected override Duration DivideItems(Duration numerator, Duration denominator) => Duration.FromNanoseconds(numerator / denominator);

        ///<inheritdoc/>
        protected override Duration MultiplyItems(Duration left, Duration right) => Duration.FromNanoseconds(left.TotalNanoseconds * right.TotalNanoseconds);

        ///<inheritdoc/>
        protected override Duration SubtractItems(Duration left, Duration right) => left - right;

        protected override ToStringMethod<Duration> StatToString { get; } = d => d.ToString("mm:ss", null);

        protected override ToStringMethod<Duration> AggregateToString { get; } = d => d.ToString("mm:ss", null);
    }

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
        protected override double CalculateAverage() => _collection.Average();
    }

    ///<inheritdoc/>
    public class DecimalStatistic : SimpleStatistic<decimal>
    {
        ///<inheritdoc/>
        public DecimalStatistic(string name) : base(name)
        { }

        ///<inheritdoc/>
        public DecimalStatistic(string name, IEnumerable<decimal> collection) : base(name, collection)
        { }

        ///<inheritdoc/>
        protected override decimal AddItems(decimal left, decimal right) => left + right;

        ///<inheritdoc/>
        protected override decimal DivideItems(decimal numerator, decimal denominator) => numerator / denominator;

        ///<inheritdoc/>
        protected override decimal MultiplyItems(decimal left, decimal right) => left * right;

        ///<inheritdoc/>
        protected override decimal SubtractItems(decimal left, decimal right) => left - right;

        ///<inheritdoc/>
        protected override decimal CalculateAverage() => _collection.Average();
    }

    ///<inheritdoc/>
    public class IntStatistic : Statistic<int, double>
    {
        ///<inheritdoc/>
        public IntStatistic(string name) : base(name)
        { }

        ///<inheritdoc/>
        public IntStatistic(string name, IEnumerable<int> collection) : base(name, collection)
        { }

        ///<inheritdoc/>
        protected override int AddItems(int left, int right) => left + right;

        ///<inheritdoc/>
        protected override int DivideItems(int numerator, int denominator) => numerator / denominator;

        ///<inheritdoc/>
        protected override int MultiplyItems(int left, int right) => left * right;

        ///<inheritdoc/>
        protected override int SubtractItems(int left, int right) => left - right;

        ///<inheritdoc/>
        protected override double CalculateAverage() => _collection.Average();
    }

    ///<inheritdoc/>
    public class LongStatistic : Statistic<long, double>
    {
        ///<inheritdoc/>
        public LongStatistic(string name) : base(name)
        { }

        ///<inheritdoc/>
        public LongStatistic(string name, IEnumerable<long> collection) : base(name, collection)
        { }

        ///<inheritdoc/>
        protected override long AddItems(long left, long right) => left + right;

        ///<inheritdoc/>
        protected override long DivideItems(long numerator, long denominator) => numerator / denominator;

        ///<inheritdoc/>
        protected override long MultiplyItems(long left, long right) => left * right;

        ///<inheritdoc/>
        protected override long SubtractItems(long left, long right) => left - right;

        ///<inheritdoc/>
        protected override double CalculateAverage() => _collection.Average();
    }

}
