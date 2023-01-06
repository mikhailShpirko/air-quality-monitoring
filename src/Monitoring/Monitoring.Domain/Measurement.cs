namespace Monitoring.Domain
{
    public sealed record Measurement
    {
        public decimal PM2_5 { get; init; }
        public decimal PM10 { get; init; }
        public DateTime Timestamp { get; init; }
    }
}