using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Measurement.API.DTO
{
    public sealed class Measurement
    {
        [JsonPropertyName("pm2_5")]
        [Required]
        [Range(0, double.MaxValue)]
        public decimal PM2_5 { get; init; }

        [JsonPropertyName("pm10")]
        [Required]
        [Range(0, double.MaxValue)]
        public decimal PM10 { get; init; }

        [JsonPropertyName("timestamp")]
        [Required]
        public DateTime Timestamp { get; init; }
    }
}
