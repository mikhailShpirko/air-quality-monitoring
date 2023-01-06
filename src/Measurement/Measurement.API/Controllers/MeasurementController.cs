using Measurement.API.Requests;
using Measurement.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Measurement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public class MeasurementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MeasurementController> _logger;

        public MeasurementController(IMediator mediator,
            ILogger<MeasurementController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{from}/{to}")]
        [SwaggerOperation(Summary = "Get Measurements for period", Description = "Get Measurements for period between from and to timespans")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DTO.Measurement>))]    
        public async Task<IActionResult> GetMeasurementsForPeriodAsync(DateTime from,
            DateTime to,
            CancellationToken cancellationToken)
        {
            try
            {
                var getMeasurementsForPeriodRequest = new GetMeasurementsForPeriodRequest(from, to);
                var measurementsForPeriod = await _mediator.Send(getMeasurementsForPeriodRequest, cancellationToken);

                return Ok(measurementsForPeriod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving measurements for period {from} - {to}", from, to);
                return InternalServerError(ex.Message);
            }            
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Save Measurement", Description = "Save Measurement")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveMeasurementAsync(DTO.Measurement measurement,
            CancellationToken cancellationToken)
        {
            try
            {
                var saveMeasurementRequest = new SaveMeasurementRequest(measurement);
                await _mediator.Send(saveMeasurementRequest, cancellationToken);

                return Ok();
            }
            catch (InvalidMeasurementValueException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error saving measurement {measurement}", measurement);
                return InternalServerError(ex.Message);
            }         
        }

        [HttpDelete("{fromDate}/{toDate}")]
        [SwaggerOperation(Summary = "Delete Measurements for period", Description = "Delete Measurements for period between from and to timespans")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMeasurementsForPeriodAsync(DateTime from,
            DateTime to,
            CancellationToken cancellationToken)
        {
            try
            {        
                var deleteMeasurementsForPeriodRequest = new DeleteMeasurementsForPeriodRequest(from, to);
                await _mediator.Send(deleteMeasurementsForPeriodRequest, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting measurements for period {from} - {to}", from, to);
                return InternalServerError(ex.Message);
            }
        }

        private IActionResult InternalServerError(string message)
        {
            return StatusCode(500, message);
        }
    }
}
