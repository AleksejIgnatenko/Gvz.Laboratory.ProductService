using Confluent.Kafka;
using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Dto;
using System.Text.Json;

namespace Gvz.Laboratory.ProductService.Kafka
{
    public class ProductKafkaProducer : IProductKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public ProductKafkaProducer(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task SendToKafkaAsync(ProductDto product, string topic)
        {
            var serializedProduct = JsonSerializer.Serialize(product);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedProduct });
        }

        public async Task SendToKafkaAsync(List<Guid> ids, string topic)
        {
            var serializedId = JsonSerializer.Serialize(ids);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedId });
        }
    }
}
