namespace Measurement.Domain
{
    public sealed record Measurement
    {
        public PositiveDecimal PM2_5 { get; private set; }
        public PositiveDecimal PM10 { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Measurement(decimal pm2_5,
            decimal pm10,
            DateTime timestamp)
        {
            PM2_5 = new PositiveDecimal(pm2_5);
            PM10 = new PositiveDecimal(pm10);
            Timestamp = timestamp;
        }
    }
}