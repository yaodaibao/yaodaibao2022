using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace iTR.LibCore
{
    public class KafkaService : BackgroundService
    {
        private readonly IConsumer<string, long> kafkaConsumer;
        private readonly string topic;

        public KafkaService(IConfiguration config)
        {
            var consumerConfig = new ConsumerConfig();

            config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
            this.topic = config.GetValue<string>("Kafka:ELKLogTopic");
            this.kafkaConsumer = new ConsumerBuilder<string, long>(consumerConfig).Build();
        }

        ////发布消息,如果发送到ELK 日志系统
        //public async Task PublishAsync(string topicName, Message<Null, string> message)
        //{
        //    var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

        //    // If serializers are not specified, default serializers from
        //    // `Confluent.Kafka.Serializers` will be automatically used where
        //    // available. Note: by default strings are encoded as UTF8.
        //    using (var p = new ProducerBuilder<Null, string>(config).Build())
        //    {
        //        try
        //        {
        //            var dr = await p.ProduceAsync(topicName, message);
        //        }
        //        catch (ProduceException<Null, string> e)
        //        {
        //            var dr = await p.ProduceAsync(topicName, new Message<Null, string>() { Value = e.Message });
        //        }
        //    }
        //}

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            kafkaConsumer.Subscribe(this.topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = this.kafkaConsumer.Consume(cancellationToken);

                    // Handle message...
                    Console.WriteLine($"{cr.Message.Key}: {cr.Message.Value}ms");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
            }
        }

        public override void Dispose()
        {
            this.kafkaConsumer.Close(); // Commit offsets and leave the group cleanly.
            this.kafkaConsumer.Dispose();

            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => StartConsumerLoop(stoppingToken)).Start();

            return Task.CompletedTask;
        }
    }
}