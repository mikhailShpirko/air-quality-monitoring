using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement.Domain.Tests.Unit
{
    public class MeasurementTests
    {
        [Fact]
        public void Ctor_MockValues_ValueeProperlySet()
        {            
            var pm2_5 = 5;
            var pm10 = 10;
            var timestamp = DateTime.Now;

            var measurement = new Measurement(pm2_5, pm10, timestamp);

            measurement.PM2_5.Value.Should().Be(pm2_5);
            measurement.PM10.Value.Should().Be(pm10);
            measurement.Timestamp.Should().Be(timestamp);
        }
    }
}
