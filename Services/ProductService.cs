using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Dto;
using Gvz.Laboratory.ProductService.Exceptions;
using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductKafkaProducer _productKafkaProducer;

        public ProductService(IProductRepository productRepository, IProductKafkaProducer productKafkaProducer)
        {
            _productRepository = productRepository;
            _productKafkaProducer = productKafkaProducer;
        }

        public async Task<Guid> CreateProductAsync(Guid id, string name, List<Guid> supplierIds)
        {
            var (errors, product) = ProductModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ProductValidationException(errors);
            }

            var productId = await _productRepository.CreateProductAsync(product, supplierIds);

            ProductDto productDto = new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
            };

            await _productKafkaProducer.SendToKafkaAsync(productDto, "add-product-topic");

            return productId;
        }

        public async Task<(List<ProductModel> products, int numberProducts)> GetProductsForPageAsync(int pageNumber)
        {
            return await _productRepository.GetProductsForPageAsync(pageNumber);
        }

        public async Task<Guid> UpdateProductAsync(Guid id, string name, List<Guid> supplierIds)
        {
            var (errors, product) = ProductModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ProductValidationException(errors);
            }

            var productId = await _productRepository.UpdateProductAsync(product, supplierIds);

            ProductDto productDto = new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
            };
            await _productKafkaProducer.SendToKafkaAsync(productDto, "update-product-topic");

            return productId;
        }

        public async Task DeleteProductAsync(List<Guid> ids)
        {
            await _productRepository.DeleteProductsAsync(ids);
            await _productKafkaProducer.SendToKafkaAsync(ids, "delete-product-topic");
        }
    }
}
