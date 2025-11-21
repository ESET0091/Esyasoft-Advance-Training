using Confluent.Kafka;
using System.Text.Json;

namespace KafkaProducer
{
    public class DeadLetterTopicProducer
    {
        public static async Task ProduceMixedMessages()
        {
            Console.WriteLine("=== Dead Letter Topic Producer ===");
            Console.WriteLine("This produces both valid and invalid messages for DLT testing");

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                EnableIdempotence = true,
                Acks = Acks.All
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            // Valid messages
            var validMessages = new[]
            {
                new Order { OrderId = 1, ProductName = "Laptop", Price = 999.99m },
                new Order { OrderId = 2, ProductName = "Mouse", Price = 29.99m },
                new Order { OrderId = 3, ProductName = "Keyboard", Price = 79.99m }
            };

            // Invalid messages (will cause processing failures)
            var invalidMessages = new[]
            {
                "{\"OrderId\": 4, \"ProductName\": \"\", \"Price\": 49.99}", // Empty product name
                "{\"OrderId\": 5, \"ProductName\": \"Monitor\", \"Price\": -100}", // Negative price
                "INVALID_JSON{]", // Malformed JSON
                "{\"OrderId\": 6, \"ProductName\": null, \"Price\": 199.99}" // Null product name
            };

            try
            {
                Console.WriteLine("Sending VALID messages:");
                foreach (var order in validMessages)
                {
                    string jsonOrder = JsonSerializer.Serialize(order);
                    await producer.ProduceAsync("orders-processing-topic",
                        new Message<Null, string> { Value = jsonOrder });
                    Console.WriteLine($"✅ Valid: Order {order.OrderId} - {order.ProductName}");
                    await Task.Delay(500);
                }

                Console.WriteLine("\nSending INVALID messages (will go to DLT):");
                foreach (var invalidMessage in invalidMessages)
                {
                    await producer.ProduceAsync("orders-processing-topic",
                        new Message<Null, string> { Value = invalidMessage });
                    Console.WriteLine($"❌ Invalid: {invalidMessage}");
                    await Task.Delay(500);
                }

                // More valid messages
                Console.WriteLine("\nSending more VALID messages:");
                for (int i = 7; i <= 9; i++)
                {
                    var order = new Order { OrderId = i, ProductName = $"Tablet-{i}", Price = 299.99m };
                    string jsonOrder = JsonSerializer.Serialize(order);
                    await producer.ProduceAsync("orders-processing-topic",
                        new Message<Null, string> { Value = jsonOrder });
                    Console.WriteLine($"✅ Valid: Order {order.OrderId} - {order.ProductName}");
                    await Task.Delay(500);
                }
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }

            producer.Flush(TimeSpan.FromSeconds(5));
            Console.WriteLine("\nDead Letter Topic Producer completed.");
        }
    }
}