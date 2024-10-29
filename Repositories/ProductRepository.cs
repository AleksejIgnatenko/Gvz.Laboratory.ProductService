using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Entities;
using Gvz.Laboratory.ProductService.Exceptions;
using Gvz.Laboratory.ProductService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GvzLaboratoryProductServiceDbContext _context;

        public ProductRepository(GvzLaboratoryProductServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateProductAsync(ProductModel product, List<Guid> supplierIds)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductName.Equals(product.ProductName));

            if (existingProduct != null) { throw new RepositoryException("Такой продукт уже существует"); }

            var existingSuppliers = await _context.Suppliers
                .Where(s => supplierIds.Contains(s.Id))
                .ToListAsync();

            if (existingSuppliers == null) { throw new RepositoryException("Поставщик(и) не найден(ы)"); }

            var productEntity = new ProductEntity
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Suppliers = existingSuppliers,
                DateCreate = DateTime.UtcNow,
            };

            await _context.Products.AddAsync(productEntity);
            await _context.SaveChangesAsync();
            return productEntity.Id;
        }

        public async Task<(List<ProductModel> products, int numberProducts)> GetProductsForPageAsync(int pageNumber)
        {
            var productEntities = await _context.Products
                .AsNoTracking()
                .Include(p => p.Suppliers)
                .OrderByDescending(p => p.DateCreate)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            var numberProducts = await _context.Products.CountAsync();

            var products = productEntities.Select(p => ProductModel.Create(
                p.Id,
                p.ProductName,
                p.Suppliers.Select(s => SupplierModel.Create(s.Id, s.SupplierName, s.Manufacturer)).ToList(),
                false).product).ToList();

            return (products, numberProducts);
        }

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetSuppliersForProductPageAsync(Guid productId, int pageNumber)
        {
            var supplierEntities = await _context.Suppliers
                .AsNoTracking()
                .Where(s => s.Products.Any(p => p.Id == productId))
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            var numberSuppliers = await _context.Suppliers
                .CountAsync(s => s.Products.Any(p => p.Id == productId));

            var suppliers = supplierEntities.Select(s => SupplierModel.Create(s.Id, s.SupplierName, s.Manufacturer)).ToList();

            return (suppliers, numberSuppliers);
        }

        public async Task<Guid> UpdateProductAsync(ProductModel product)
        {
            await _context.Products
                .Where(p => p.Id == product.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.ProductName, product.ProductName)
                );

            return product.Id;
        }

        public async Task DeleteProductsAsync(List<Guid> ids)
        {
            await _context.Products
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
