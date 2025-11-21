using Confluent.Kafka;
using System.Text.Json;

namespace KafkaConsumer
{
    public class DltMonitorConsumer
    {
        public static void StartDltMonitoring()
        {
            Console.WriteLine("=== Dead Letter Topic Monitor ===");
            Console.WriteLine("This consumer monitors the dead letter topic for failed messages");

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "dlt-monitor-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("dead-letter-topic");

            Console.WriteLine("DLT Monitor started. Press Ctrl+C to exit.");
            Console.WriteLine("Monitoring for failed messages...\n");

            var failedMessages = new List<DLTMessage>();

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    var dltMessage = JsonSerializer.Deserialize<DLTMessage>(consumeResult.Message.Value);

                    failedMessages.Add(dltMessage);

                    Console.WriteLine($"🚨 DLT MESSAGE RECEIVED:");
                    Console.WriteLine($"   Timestamp: {dltMessage.Timestamp}");
                    Console.WriteLine($"   Error: {dltMessage.ErrorType} - {dltMessage.ErrorMessage}");
                    Console.WriteLine($"   Original Message: {dltMessage.OriginalMessage}");
                    Console.WriteLine($"   Total failures in DLT: {failedMessages.Count}");
                    Console.WriteLine($"   ---");

                    // In production, you might:
                    // - Send alert to operations team
                    // - Log to monitoring system
                    // - Trigger automatic recovery process

                    consumer.Commit(consumeResult);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("DLT Monitor stopped.");
            }
            finally
            {
                consumer.Close();

                Console.WriteLine($"\n📊 DLT MONITOR SUMMARY:");
                Console.WriteLine($"Total failed messages: {failedMessages.Count}");

                var errorGroups = failedMessages.GroupBy(f => f.ErrorType);
                foreach (var group in errorGroups)
                {
                    Console.WriteLine($"   {group.Key}: {group.Count()} messages");
                }
            }
        }
    }

    // Helper class for DLT message structure
    public class DLTMessage
    {
        public string OriginalMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
        public string Timestamp { get; set; }
        public string Topic { get; set; }
        public string StackTrace { get; set; }
    }
}