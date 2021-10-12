using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace iTR.LibCore
{
    public class KafkaClientHandle : IDisposable
    {
        private IProducer<byte[], byte[]> kafkaProducer;

        public KafkaClientHandle(IConfiguration config)
        {
            //var conf = new ProducerConfig();
            //config.GetSection("Kafka:ProducerSettings").Bind(conf);
            var conf = new ProducerConfig { BootstrapServers = "localhost:9092" };
            this.kafkaProducer = new ProducerBuilder<byte[], byte[]>(conf).Build();
        }

        public Handle Handle { get => this.kafkaProducer.Handle; }

        public void Dispose()
        {
            // Block until all outstanding produce requests have completed (with or
            // without error).
            kafkaProducer.Flush();
            kafkaProducer.Dispose();
        }
    }
}