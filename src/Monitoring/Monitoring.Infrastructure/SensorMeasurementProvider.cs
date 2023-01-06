using Monitoring.Domain;
using System.Text.Json;

namespace Monitoring.Infrastructure
{
    internal sealed class SensorMeasurementProvider : IMeasurementProvider
    {
        private readonly HttpClient _httpClient;
        private const string QueryMeasurementEndpoint = "api/query_measurement";

        public SensorMeasurementProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Measurement> GetCurrentMeasurementAsync(CancellationToken cancellationToken)
        {      
            var queryMeasurementResponse = await _httpClient.GetAsync(QueryMeasurementEndpoint, cancellationToken);
            var queryMeasurementResponseContent = await queryMeasurementResponse.Content.ReadAsStringAsync();

            if (!queryMeasurementResponse.IsSuccessStatusCode)
            {
                throw new InvalidApiResponseException($"API {_httpClient.BaseAddress} responded with error {queryMeasurementResponse.StatusCode}: {queryMeasurementResponseContent}");
            }

            var sensorMeasurement = JsonSerializer
                .Deserialize<SensorMeasurementDTO>(queryMeasurementResponseContent);

            if (sensorMeasurement == null)
            {
                throw new InvalidApiResponseException($"API {_httpClient.BaseAddress} responded with: {queryMeasurementResponseContent}");
            }

            return new Measurement
            {
                PM10 = sensorMeasurement.PM10,
                PM2_5 = sensorMeasurement.PM2_5,
                Timestamp = DateTime.Now
            };            
        }
    }
}
