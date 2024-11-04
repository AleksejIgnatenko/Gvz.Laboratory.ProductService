using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Services
{
    public class SuppliersService : ISuppliersService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetProductSuppliersForPageAsync(Guid productId, int pageNumber)
        {
            return await _supplierRepository.GetProductSuppliersForPageAsync(productId, pageNumber);
        }

        public async Task<List<SupplierModel>> GetProductSuppliersAsync(Guid productId)
        {
            return await _supplierRepository.GetProductSuppliersAsync(productId);
        }
    }
}
