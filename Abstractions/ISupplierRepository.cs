using Gvz.Laboratory.ProductService.Dto;
using Gvz.Laboratory.ProductService.Entities;
using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface ISupplierRepository
    {
        Task<Guid> CreateSupplierAsync(SupplierDto supplier);
        Task DeleteSuppliersAsync(List<Guid> ids);
        Task<List<SupplierModel>> GetProductSuppliersAsync(Guid productId);
        Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetProductSuppliersForPageAsync(Guid productId, int pageNumber);
        Task<List<SupplierEntity>> GetSuppliersByIdsAsync(List<Guid> suppliersIds);
        Task<Guid> UpdateSupplierAsync(SupplierDto supplier);
    }
}