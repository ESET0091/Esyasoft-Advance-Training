namespace RabbitMQ.Models
{
    public class MeterReadingMessage
    {
        public string MeterSerialNo { get; set; } = string.Empty;
        public decimal EnergyConsumed { get; set; }
        public decimal Voltage { get; set; } = 220.0m;
        public decimal Current { get; set; } = 10.0m;
        public DateTime ReadingDate { get; set; } = DateTime.UtcNow;
    }
}
