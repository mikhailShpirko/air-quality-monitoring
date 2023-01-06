namespace Measurement.Domain.Tests.Unit
{
    public class PositiveDecimalTests
    {
        [Fact]
        public void Ctor_PositiveDecimalArgument_ValueProperlySet()
        {
            var positiveDecimal = new PositiveDecimal(5);
            positiveDecimal.Value.Should().Be(5);
        }

        [Fact]
        public void Ctor_0DecimalArgument_ValueProperlySet()
        {
            var positiveDecimal = new PositiveDecimal(0);
            positiveDecimal.Value.Should().Be(0);
        }

        [Fact]
        public void Ctor_NegativeDecimalArgument_ExceptionThrown()
        {
            Action act = () => new PositiveDecimal(-1);

            act.Should().Throw<InvalidMeasurementValueException>("Measurement Value should be >= 0. Current value: -1");
        }
    }
}