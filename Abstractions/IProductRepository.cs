using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface IProductRepository
    {
        Task<Guid> CreateProductAsync(ProductModel product, List<Guid> supplierIds);
        Task DeleteProductsAsync(List<Guid> ids);
        Task<(List<ProductModel> products, int numberProducts)> GetProductsForPageAsync(int pageNumber);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForProductPageAsync(Guid productId, int pageNumber);
        Task<Guid> UpdateProductAsync(ProductModel product);
    }
}