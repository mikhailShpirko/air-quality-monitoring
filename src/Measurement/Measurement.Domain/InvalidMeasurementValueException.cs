namespace Measurement.Domain
{
    public sealed class InvalidMeasurementValueException : Exception
    {
        public InvalidMeasurementValueException(decimal value)
            : base($"Measurement Value should be >= 0. Current value: {value}")
        {

        }
    }
}
