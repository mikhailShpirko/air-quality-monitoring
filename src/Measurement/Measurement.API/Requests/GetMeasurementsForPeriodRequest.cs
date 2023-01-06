using MediatR;

namespace Measurement.API.Requests
{
    public sealed class GetMeasurementsForPeriodRequest 
        : IRequest<IEnumerable<DTO.Measurement>>
    {
        public readonly DateTime From;
        public readonly DateTime To;

        public GetMeasurementsForPeriodRequest(DateTime from,
            DateTime to)
        {
            From = from;
            To = to;
        }
    }
}
