using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface ISuppliersService
    {
        Task<List<SupplierModel>> GetProductSuppliersAsync(Guid productId);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetProductSuppliersForPageAsync(Guid productId, int pageNumber);
    }
}