using Monitoring.Domain;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Monitoring.Infrastructure.Tests.Integration
{
    public class MeasurementApiRepositoryTests
    {
        [Fact]
        public async Task SaveAsync_ApiRespondsWithSuccess_NoExceptionThrown()
        {
            using var mockServer = WireMockServer.Start();
            mockServer.Given(Request.Create().WithPath("/api/measurement").UsingPost())
                .RespondWith(Response
                    .Create()
                    .WithSuccess());

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(mockServer.Urls[0])
            };

            var repository = new MeasurementApiRepository(httpClient);

            Func<Task> act = () => repository.SaveAsync(new Measurement(), default);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task SaveAsync_ApiRespondsWithError_ExceptionThrown()
        {
            using var mockServer = WireMockServer.Start();
            mockServer.Given(Request.Create().WithPath("/api/measurement").UsingPost())
                .RespondWith(Response
                    .Create()
                    .WithStatusCode(500));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(mockServer.Urls[0])
            };

            var repository = new MeasurementApiRepository(httpClient);

            Func<Task> act = () => repository.SaveAsync(new Measurement(), default);

            await act.Should().ThrowAsync<InvalidApiResponseException>();
        }
    }
}