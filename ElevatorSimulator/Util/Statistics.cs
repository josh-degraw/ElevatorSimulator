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
        public DurationStatistic() : base()
        { }

        ///<inheritdoc/>
        public DurationStatistic(IEnumerable<Duration> collection) : base(collection)
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
    }

    ///<inheritdoc/>
    public class DoubleStatistic : SimpleStatistic<double>
    {
        ///<inheritdoc/>
        public DoubleStatistic() : base()
        { }

        ///<inheritdoc/>
        public DoubleStatistic(IEnumerable<double> collection) : base(collection)
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
        public DecimalStatistic() : base()
        { }

        ///<inheritdoc/>
        public DecimalStatistic(IEnumerable<decimal> collection) : base(collection)
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
        public IntStatistic() : base()
        { }

        ///<inheritdoc/>
        public IntStatistic(IEnumerable<int> collection) : base(collection)
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
        public LongStatistic() : base()
        { }

        ///<inheritdoc/>
        public LongStatistic(IEnumerable<long> collection) : base(collection)
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
