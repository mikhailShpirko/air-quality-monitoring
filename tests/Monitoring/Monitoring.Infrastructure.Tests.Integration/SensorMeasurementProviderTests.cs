using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Monitoring.Infrastructure.Tests.Integration
{
    public class SensorMeasurementProviderTests
    {
        [Fact]
        public async Task GetCurrentMeasurementAsync_ApiRespondsWithMeasurement_MeasurementObjectMappedAndReturned()
        {
            using var mockServer = WireMockServer.Start();
            mockServer.Given(Request.Create().WithPath("/api/query_measurement").UsingGet())
                .RespondWith(Response
                    .Create()
                    .WithBodyAsJson(new
                    {
                        pm2_5 = 100,
                        pm10 = 200
                    }));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(mockServer.Urls[0])
            };

            var provider = new SensorMeasurementProvider(httpClient);
            var measurement = await provider.GetCurrentMeasurementAsync(default);

            measurement.Should().NotBeNull();
            measurement.PM2_5.Should().Be(100);
            measurement.PM10.Should().Be(200);
        }

        [Fact]
        public async Task GetCurrentMeasurementAsync_ApiRespondsWithError_ExceptionThrown()
        {
            using var mockServer = WireMockServer.Start();
            mockServer.Given(Request.Create().WithPath("/api/query_measurement").UsingGet())
                .RespondWith(Response
                    .Create()
                    .WithStatusCode(500));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(mockServer.Urls[0])
            };

            var provider = new SensorMeasurementProvider(httpClient);

            Func<Task> act = () => provider.GetCurrentMeasurementAsync(default);

            await act.Should().ThrowAsync<InvalidApiResponseException>();
        }
    }
}