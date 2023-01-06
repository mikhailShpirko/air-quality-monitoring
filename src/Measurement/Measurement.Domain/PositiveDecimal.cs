namespace Measurement.Domain
{
    public sealed record PositiveDecimal
    {
        public decimal Value { get; private set; }
        public PositiveDecimal(decimal value)
        {
            if (value < 0)
            {
                throw new InvalidMeasurementValueException(value);
            }
            Value = value;
        }
    }
}
