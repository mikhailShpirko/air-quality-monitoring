using Microsoft.EntityFrameworkCore;

namespace Measurement.Infrastructure.Tests.Integration
{
    public class MeasurementEFRepositoryTests
    {
        [Fact]
        public async void DeleteForPeriodAsync_RangeToDeleteSingeRecord_SingleRecordDeleted()
        {
            var date = DateTime.Now;
            using var fixture = new MeasurementDataFixture(date);

            var repo = new MeasurementEFRepository(fixture.DbContext);

            await repo
                .DeleteForPeriodAsync(date.AddSeconds(-1),
                    date.AddSeconds(1), 
                    default);

            var remainingRecords = await fixture.DbContext.Measurements.ToListAsync();

            remainingRecords.Count().Should().Be(2);
            remainingRecords.All(x => x.TimestampTicks != date.Ticks).Should().BeTrue();
        }

        [Fact]
        public async void GetPeriodAsync_RangeToSelectSingeRecord_SingleRecordReturned()
        {
            var date = DateTime.Now;
            using var fixture = new MeasurementDataFixture(date);

            var repo = new MeasurementEFRepository(fixture.DbContext);

            var result = await repo
                .GetForPeriodAsync(date.AddSeconds(-1),
                    date.AddSeconds(1),
                    default);

            result.Count().Should().Be(1);

            var measurement = result.First();
            measurement.PM2_5.Value.Should().Be(1);
            measurement.PM10.Value.Should().Be(2);
            measurement.Timestamp.Should().Be(date);            
        }


        [Fact]
        public async void SaveAsync_RecordToCreate_SingleRecordCreated()
        {
            var date = DateTime.Now;
            var newRecordTimestamp = date.AddDays(3);
            using var fixture = new MeasurementDataFixture(date);

            var repo = new MeasurementEFRepository(fixture.DbContext);

            var measurement = new Domain.Measurement(9, 10, newRecordTimestamp);

            await repo.SaveAsync(measurement, default);

            var allRecords = await fixture.DbContext.Measurements.ToListAsync();

            allRecords.Count().Should().Be(4);
            allRecords
                .SingleOrDefault(x => x.TimestampTicks == newRecordTimestamp.Ticks &&
                    x.PM2_5 == 9 &&
                    x.PM10 == 10)
                .Should().NotBeNull();
        }
    }
}