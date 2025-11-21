////using Confluent.Kafka;
////using KafkaProducer;
////using System.Text.Json;

////var config = new ProducerConfig
////{
////    // Replace "localhost:9092" with your actual broker addresses.
////    BootstrapServers = "localhost:9092"
////};

////using var producer = new ProducerBuilder<Null, string>(config).Build();
////var order = new Order { OrderId = 124, ProductName = "Desktop", Price = 1300.50m };
////string jsonOrder = JsonSerializer.Serialize(order);

////try
////{
////    var deliveryReport = await producer.ProduceAsync("my-third-topic", new Message<Null, string>
////    {
////        Value = jsonOrder
////    });

////    Console.WriteLine($"Message sent to partition: {deliveryReport.Partition}, offset: {deliveryReport.Offset}");
////}
////catch (ProduceException<Null, string> e)
////{
////    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
////}

////// Ensure all buffered messages are sent.
////producer.Flush(TimeSpan.FromSeconds(10));



using Confluent.Kafka;
using KafkaProducer;
using System.Text.Json;

Console.WriteLine("=== Kafka Producer ===");
Console.WriteLine("Choose producer type:");
Console.WriteLine("1 - Basic Producer (Your existing code)");
Console.WriteLine("2 - At-Most-Once Producer");
Console.WriteLine("3 - At-Least-Once Producer");
Console.WriteLine("4 - Exactly-Once Producer");
Console.WriteLine("5 - Idempotent Producer");
Console.WriteLine("6 - Non-Idempotent Producer");
Console.WriteLine("7 - Transactional Producer");
Console.WriteLine("8 - Transactional Producer with Failure Simulation");
Console.WriteLine("9 - Dead Letter Topic Producer (Mixed valid/invalid messages)");
Console.WriteLine("10 - Scheme Registory using Docker)");


var choice = Console.ReadLine();
User user = new User() { Address = "Manglore", Age = 19, CreatedAt = DateTime.Now, Email = "gopal@gmail.com", Name = "Gopal", PhoneNumber = "73897525348", Size = 78 };

switch (choice)
{
    case "1":
        await RunBasicProducer();
        break;
    case "2":
        await AtMostOnceProducer.ProduceMessages();
        break;
    case "3":
        await AtLeastOnceProducer.ProduceMessages();
        break;
    case "4":
        await ExactlyOnceProducer.ProduceMessages();
        break;
    case "5":
        await IdempotentProducer.ProduceMessages();
        break;
    case "6":
        await NonIdempotentProducer.ProduceMessages();
        break;
    case "7":
        await TransactionalProducer.ProduceWithTransaction();
        break;
    case "8":
        await TransactionalProducerWithFailure.ProduceWithSimulatedFailure();
        break;
    case "9":
        await DeadLetterTopicProducer.ProduceMixedMessages();
        break;
    case "10":
        await JsonSchemaProducer.ProduceUserMessageAsync("app-agent", user);
        break;
    default:
        Console.WriteLine("Invalid choice, running Basic Producer");
        await RunBasicProducer();
        break;
}

// Your existing basic producer code
static async Task RunBasicProducer()
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };

    using var producer = new ProducerBuilder<Null, string>(config).Build();
    var order = new Order { OrderId = 124, ProductName = "Desktop", Price = 1300.50m };
    string jsonOrder = JsonSerializer.Serialize(order);

    try
    {
        var deliveryReport = await producer.ProduceAsync("my-third-topic", new Message<Null, string>
        {
            Value = jsonOrder
        });

        Console.WriteLine($"Message sent to partition: {deliveryReport.Partition}, offset: {deliveryReport.Offset}");
    }
    catch (ProduceException<Null, string> e)
    {
        Console.WriteLine($"Delivery failed: {e.Error.Reason}");
    }

    producer.Flush(TimeSpan.FromSeconds(10));
    Console.WriteLine("Basic Producer completed.");
}




//using Confluent.Kafka;
//using Streamiz.Kafka.Net;
//using Streamiz.Kafka.Net.SerDes;
//using Streamiz.Kafka.Net.Stream;
//using Streamiz.Kafka.Net.Table;
//using System;
//using System.Threading.Tasks;

//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        // 1. Configure the Streamiz application
//        var config = new StreamConfig<StringSerDes, StringSerDes>();
//        config.ApplicationId = "sensor-data-processor-app";
//        config.BootstrapServers = "localhost:9092";
//        config.AutoOffsetReset = AutoOffsetReset.Earliest;

//        // 2. Build the stream processing topology
//        var builder = new StreamBuilder();
//        builder.Stream<string, string>("sensor-readings-topic")
//               // Step A: Filter out readings below 10
//               .Filter(static (key, value) => double.Parse(value) >= 10.0)
//               // Step B: Group by the sensor ID
//               .GroupByKey()
//               // Step C: Count the readings in a 1-minute tumbling window
//               .WindowedBy(TumblingWindowOptions.Of(TimeSpan.FromMinutes(1)))
//               .Count() // Simplified approach
//                        // Step D: Convert the result back to a stream and send it to a new topic
//               .ToStream((key, value) => $"{key.Key}_{key.Window.Start}_{key.Window.End}")
//               .To("sensor-counts-topic");

//        // 3. Create and start the Streamiz application
//        var topology = builder.Build();
//        var stream = new KafkaStream(topology, config);

//        // Start the stream processing and wait for a cancellation signal
//        await stream.StartAsync();
//    }
//}