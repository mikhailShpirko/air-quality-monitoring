using System.Text.Json.Serialization;

namespace Monitoring.Infrastructure
{
    internal sealed record SensorMeasurementDTO
    {
        [JsonPropertyName("pm2_5")]
        public decimal PM2_5 { get; init; }

        [JsonPropertyName("pm10")]
        public decimal PM10 { get; init; }
    }
}
