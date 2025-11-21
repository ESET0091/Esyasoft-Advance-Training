using Confluent.Kafka;
using System.Text.Json;

namespace KafkaConsumer.Consumers
{
    public class DeadLetterTopicConsumer
    {
        private static IProducer<Null, string> _dltProducer;
        private static Dictionary<string, int> _retryAttempts = new Dictionary<string, int>();

        public static void StartConsuming()
        {
            Console.WriteLine("=== Dead Letter Topic Consumer ===");
            Console.WriteLine("This consumer processes valid messages and sends invalid ones to DLT");

            // Main consumer config
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "dlt-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            // DLT producer config
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _dltProducer = new ProducerBuilder<Null, string>(producerConfig).Build();

            consumer.Subscribe("orders-processing-topic");

            Console.WriteLine("DLT Consumer started. Press Ctrl+C to exit.");
            Console.WriteLine("Will process valid messages and send invalid ones to dead-letter-topic");

            var processedCount = 0;
            var dltCount = 0;

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume();

                    // Check if consumeResult is null (timeout or no message)
                    if (consumeResult == null)
                    {
                        continue; // Skip and continue polling
                    }

                    var message = consumeResult.Message.Value;

                    try
                    {
                        // Try to process the message
                        ProcessOrderMessage(message);
                        processedCount++;
                        Console.WriteLine($"✅ PROCESSED: {message}");
                        Console.WriteLine($"📊 Stats: {processedCount} processed, {dltCount} to DLT");

                        // Commit offset since message was processed successfully
                        consumer.Commit(consumeResult);
                    }
                    catch (Exception ex)
                    {
                        // Message processing failed - handle with retry logic
                        HandleFailedMessage(consumeResult.Message.Value, ex, ref dltCount);

                        // Still commit offset to move past this message (after DLT handling)
                        consumer.Commit(consumeResult);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("DLT Consumer stopped.");
            }
            finally
            {
                consumer.Close();
                _dltProducer?.Dispose();
                Console.WriteLine($"\n📊 FINAL: {processedCount} processed successfully, {dltCount} sent to DLT");
            }
        }

        private static void ProcessOrderMessage(string message)
        {
            // Validate JSON structure first
            if (!IsValidJson(message))
            {
                throw new InvalidOperationException("Invalid JSON format");
            }

            var order = JsonSerializer.Deserialize<Order>(message);

            // Business logic validation
            if (order.OrderId <= 0)
            {
                throw new ArgumentException($"Invalid OrderId: {order.OrderId}");
            }

            if (string.IsNullOrWhiteSpace(order.ProductName))
            {
                throw new ArgumentException("Product name cannot be empty or null");
            }

            if (order.Price <= 0)
            {
                throw new ArgumentException($"Invalid price: {order.Price}. Price must be positive");
            }

            // Simulate business processing
            Console.WriteLine($"   Processing order {order.OrderId}: {order.ProductName} for ${order.Price}");
            Thread.Sleep(100); // Simulate processing time
        }

        private static bool IsValidJson(string jsonString)
        {
            try
            {
                JsonSerializer.Deserialize<Order>(jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void HandleFailedMessage(string messageValue, Exception error, ref int dltCount)
        {
            var messageKey = messageValue.GetHashCode().ToString();
            var maxRetries = 2;

            // Track retry attempts
            if (!_retryAttempts.ContainsKey(messageKey))
            {
                _retryAttempts[messageKey] = 0;
            }

            _retryAttempts[messageKey]++;

            Console.WriteLine($"❌ Processing failed (attempt {_retryAttempts[messageKey]}): {error.Message}");

            if (_retryAttempts[messageKey] < maxRetries)
            {
                Console.WriteLine($"   Will retry message...");
                // In a real scenario, you might re-queue or wait before retry
                Thread.Sleep(1000);

                // For demo, we'll simulate retry by reprocessing
                try
                {
                    ProcessOrderMessage(messageValue);
                    Console.WriteLine($"✅ RETRY SUCCESSFUL on attempt {_retryAttempts[messageKey]}");
                    _retryAttempts.Remove(messageKey);
                    return;
                }
                catch (Exception retryEx)
                {
                    Console.WriteLine($"❌ Retry also failed: {retryEx.Message}");
                }
            }

            // Max retries exceeded - send to Dead Letter Topic
            SendToDeadLetterTopic(messageValue, error);
            dltCount++;
            _retryAttempts.Remove(messageKey);
        }

        private static void SendToDeadLetterTopic(string originalMessageValue, Exception error)
        {
            var dltMessage = new
            {
                OriginalMessage = originalMessageValue,
                ErrorMessage = error.Message,
                ErrorType = error.GetType().Name,
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Topic = "orders-processing-topic",
                StackTrace = error.StackTrace
            };

            string dltPayload = JsonSerializer.Serialize(dltMessage);

            try
            {
                _dltProducer.Produce("dead-letter-topic",
                    new Message<Null, string> { Value = dltPayload });

                Console.WriteLine($"⚰️ Sent to DLT: {error.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💀 FAILED to send to DLT: {ex.Message}");
                // In production, you should have a fallback strategy (logging, alerting, etc.)
            }
        }
    }
}