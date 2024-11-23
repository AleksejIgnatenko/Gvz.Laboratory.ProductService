using Gvz.Laboratory.ProductService.Entities;
using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface IProductRepository
    {
        Task<Guid> CreateProductAsync(ProductModel product, List<Guid> supplierIds);
        Task DeleteProductsAsync(List<Guid> ids);
        Task<List<ProductModel>> GetProductsAsync();
        Task<ProductEntity?> GetProductEntityByIdAsync(Guid productId);
        Task<(List<ProductModel> products, int numberProducts)> GetProductsForPageAsync(int pageNumber);
        Task<(List<ProductModel> products, int numberProducts)> SearchProductsAsync(string searchQuery, int pageNumber);
        Task<Guid> UpdateProductAsync(ProductModel product, List<Guid> supplierIds);
    }
}