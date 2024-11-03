using Gvz.Laboratory.ProductService.Dto;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface IProductKafkaProducer
    {
        Task SendToKafkaAsync(ProductDto product, string topic);
        Task SendToKafkaAsync(List<Guid> ids, string topic);
    }
}