using System.Text.Json.Serialization;

namespace RabbitMQ
{
    public class MeterReadingMessage
    {
        [JsonPropertyName("meterSerialNo")]
        public string MeterSerialNo { get; set; } = string.Empty;

        [JsonPropertyName("energyConsumed")]
        public decimal EnergyConsumed { get; set; }

        [JsonPropertyName("voltage")]
        public decimal Voltage { get; set; } = 220.0m;

        [JsonPropertyName("current")]
        public decimal Current { get; set; } = 10.0m;

        [JsonPropertyName("readingDate")]
        public DateTime ReadingDate { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}