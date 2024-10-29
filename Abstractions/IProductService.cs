using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface IProductService
    {
        Task<Guid> CreateProductAsync(Guid id, string name, List<Guid> supplierIds);
        Task DeleteProductAsync(List<Guid> ids);
        Task<(List<ProductModel> products, int numberProducts)> GetProductsForPageAsync(int pageNumber);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForProductPageAsync(Guid productId, int pageNumber);
        Task<Guid> UpdateProductAsync(Guid id, string name);
    }
}