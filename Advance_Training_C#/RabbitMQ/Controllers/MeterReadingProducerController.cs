
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.services;

namespace RabbitMQ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingProducerController : ControllerBase
    {
        private readonly IRabbitMQProducerService _producerService;
        private readonly ILogger<MeterReadingProducerController> _logger;

        public MeterReadingProducerController(IRabbitMQProducerService producerService, ILogger<MeterReadingProducerController> logger)
        {
            _producerService = producerService;
            _logger = logger;
        }

        [HttpPost("send-reading")]
        public IActionResult SendMeterReading([FromBody] MeterReadingMessage reading)
        {
            try
            {
                if (string.IsNullOrEmpty(reading.MeterSerialNo))
                    return BadRequest("Meter serial number is required");

                if (reading.EnergyConsumed < 0)
                    return BadRequest("Energy consumed cannot be negative");

                _producerService.SendMeterReading(reading);

                return Ok(new
                {
                    message = "Meter reading sent successfully",
                    meterSerialNo = reading.MeterSerialNo,
                    energyConsumed = reading.EnergyConsumed,
                    timestamp = reading.Timestamp
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending meter reading");
                return StatusCode(500, "Error sending meter reading");
            }
        }

        [HttpPost("generate-test-data")]
        public IActionResult GenerateTestData()
        {
            try
            {
                var random = new Random();
                var meters = new[] { "MTR001", "MTR002", "MTR003", "MTR004" };

                foreach (var meter in meters)
                {
                    var reading = new MeterReadingMessage
                    {
                        MeterSerialNo = meter,
                        EnergyConsumed = (decimal)Math.Round(1000 + random.NextDouble() * 1000, 2),
                        Voltage = 220 + random.Next(-5, 5),
                        Current = 10 + random.Next(-2, 2),
                        ReadingDate = DateTime.UtcNow.AddHours(-random.Next(1, 24)),
                        Timestamp = DateTime.UtcNow
                    };

                    _producerService.SendMeterReading(reading);
                }

                return Ok(new { message = "Test data generated and sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating test data");
                return StatusCode(500, "Error generating test data");
            }
        }
    }
}