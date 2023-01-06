using Monitoring.Domain;
using System.Net.Http.Json;

namespace Monitoring.Infrastructure
{
    internal sealed class MeasurementApiRepository : IMeasurementRepository
    {
        private readonly HttpClient _httpClient;
        private const string MeasurementEndpoint = "api/measurement";

        public MeasurementApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SaveAsync(Measurement measurement, CancellationToken cancellationToken)
        {
            var measurementDto = new MeasurementDTO
            {
                PM10 = measurement.PM10,
                PM2_5 = measurement.PM2_5,
                Timestamp = measurement.Timestamp
            };

            var postMeasurementResponse = await _httpClient.PostAsJsonAsync(MeasurementEndpoint, measurementDto, cancellationToken);
            
            if (postMeasurementResponse.IsSuccessStatusCode)
            {
                return;
            }

            var postMeasurementResponseContent = await postMeasurementResponse.Content.ReadAsStringAsync();

            throw new InvalidApiResponseException($"API {_httpClient.BaseAddress} responded with error {postMeasurementResponse.StatusCode}: {postMeasurementResponseContent}");
        }
    }
}
