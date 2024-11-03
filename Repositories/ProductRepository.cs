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
        private readonly ISupplierRepository _supplierRepository;

        public ProductRepository(GvzLaboratoryProductServiceDbContext context, ISupplierRepository supplierRepository)
        {
            _context = context;
            _supplierRepository = supplierRepository;
        }

        public async Task<Guid> CreateProductAsync(ProductModel product, List<Guid> supplierIds)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductName.Equals(product.ProductName));

            if (existingProduct != null) { throw new RepositoryException("Такой продукт уже существует"); }

            var existingSuppliers = await _supplierRepository.GetSuppliersByIdsAsync(supplierIds)
                ?? throw new RepositoryException("Поставщик(и) не был(и) найден(ы)");

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
                .OrderByDescending(p => p.DateCreate)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            if (!productEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                productEntities = await _context.Products
                    .AsNoTracking()
                    .OrderByDescending(p => p.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberProducts = await _context.Products.CountAsync();

            var products = productEntities.Select(p => ProductModel.Create(
                p.Id,
                p.ProductName,
                false).product).ToList();

            return (products, numberProducts);
        }

        public async Task<Guid> UpdateProductAsync(ProductModel product, List<Guid> supplierIds)
        {
            var supplierEntities = await _supplierRepository.GetSuppliersByIdsAsync(supplierIds)
                ?? throw new RepositoryException("Поставщик(и) не был(и) найден(ы)");

            var productEntity = await _context.Products
                .Include(p => p.Suppliers)
                .FirstOrDefaultAsync(p => p.Id == product.Id)
                ?? throw new RepositoryException("Продукт не найден");

            productEntity.ProductName = product.ProductName;

            productEntity.Suppliers.Clear();
            productEntity.Suppliers.AddRange(supplierEntities);

            await _context.SaveChangesAsync();

            return productEntity.Id;
        }

        public async Task DeleteProductsAsync(List<Guid> ids)
        {
            var productEntities = await _context.Products
                .Include(p => p.Suppliers)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            foreach (var productEntity in productEntities)
            {
                productEntity.Suppliers.Clear();
            }

            await _context.SaveChangesAsync();

            await _context.Products
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
