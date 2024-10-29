using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Exceptions;
using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> CreateProductAsync(Guid id, string name, List<Guid> supplierIds)
        {
            var (errors, product) = ProductModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ProductValidationException(errors);
            }

            var productId = await _productRepository.CreateProductAsync(product, supplierIds);

            //mapping
            //send to kafka

            return productId;
        }

        public async Task<(List<ProductModel> products, int numberProducts)> GetProductsForPageAsync(int pageNumber)
        {
            return await _productRepository.GetProductsForPageAsync(pageNumber);
        }

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForProductPageAsync(Guid productId, int pageNumber)
        {
            return await _productRepository.GetSuppliersForProductPageAsync(productId, pageNumber);
        }

        public async Task<Guid> UpdateProductAsync(Guid id, string name)
        {
            var (errors, product) = ProductModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ProductValidationException(errors);
            }

            var productId = await _productRepository.UpdateProductAsync(product);

            //mapping
            //send to kafka

            return productId;
        }

        public async Task DeleteProductAsync(List<Guid> ids)
        {
            await _productRepository.DeleteProductsAsync(ids);
            //await _supplierKafkaProducer.SendUserToKafka(ids, "delete-supplier-topic");
        }
    }
}
