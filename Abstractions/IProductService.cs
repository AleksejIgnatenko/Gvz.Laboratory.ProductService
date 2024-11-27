using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface IProductService
    {
        Task<Guid> CreateProductAsync(Guid id, string name, string unitsOfMeasurement, List<Guid> supplierIds);
        Task DeleteProductAsync(List<Guid> ids);
        Task<List<ProductModel>> GetProductsAsync();
        Task<(List<ProductModel> products, int numberProducts)> GetProductsForPageAsync(int pageNumber);
        Task<(List<ProductModel> products, int numberProducts)> SearchProductsAsync(string searchQuery, int pageNumber);
        Task<MemoryStream> ExportProductsToExcelAsync();
        Task<Guid> UpdateProductAsync(Guid id, string name, string unitsOfMeasurement, List<Guid> supplierIds);
    }
}