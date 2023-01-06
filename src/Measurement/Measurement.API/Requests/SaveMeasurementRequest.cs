using MediatR;

namespace Measurement.API.Requests
{
    public class SaveMeasurementRequest : IRequest
    {
        public readonly DTO.Measurement Measurement;

        public SaveMeasurementRequest(DTO.Measurement measurement)
        {
            Measurement = measurement;
        }
    }
}
