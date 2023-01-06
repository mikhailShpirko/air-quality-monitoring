using MediatR;

namespace Measurement.API.Requests
{
    public sealed class DeleteMeasurementsForPeriodRequest : IRequest
    {
        public readonly DateTime From;
        public readonly DateTime To;

        public DeleteMeasurementsForPeriodRequest(DateTime from,
            DateTime to)
        {
            From = from;
            To = to;
        }
    }
}
