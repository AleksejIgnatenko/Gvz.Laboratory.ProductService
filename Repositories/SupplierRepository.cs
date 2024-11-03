using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Dto;
using Gvz.Laboratory.ProductService.Entities;
using Gvz.Laboratory.ProductService.Exceptions;
using Gvz.Laboratory.ProductService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ProductService.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly GvzLaboratoryProductServiceDbContext _context;

        public SupplierRepository(GvzLaboratoryProductServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateSupplierAsync(SupplierDto supplier)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierName.Equals(supplier.SupplierName));

            if (existingSupplier == null)
            {
                var supplierEntity = new SupplierEntity
                {
                    Id = supplier.Id,
                    SupplierName = supplier.SupplierName,
                };

                await _context.Suppliers.AddAsync(supplierEntity);
                await _context.SaveChangesAsync();
            }

            return supplier.Id;
        }

        public async Task<List<SupplierModel>> GetProductSuppliersAsync(Guid productId)
        {
            var productEntity = await _context.Products
                .Include(p => p.Suppliers)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (productEntity == null)
            {
                throw new RepositoryException("Продукт не найден");
            }

            var suppliers = productEntity.Suppliers.Select(s => SupplierModel.Create(s.Id, s.SupplierName)).ToList();

            return suppliers;
        }

        public async Task<(List<SupplierModel> suppliers, int numberSuppliers)> GetProductSuppliersForPageAsync(Guid productId, int pageNumber)
        {
            var supplierEntities = await _context.Products
                .Where(p => p.Id == productId)
                .SelectMany(p => p.Suppliers)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            var numberSuppliers = await _context.Products
                .Where(p => p.Id == productId)
                .SelectMany(p => p.Suppliers)
                .CountAsync();

            var suppliers = supplierEntities.Select(s => SupplierModel.Create(s.Id, s.SupplierName)).ToList();

            return (suppliers, numberSuppliers);
        }

        public async Task<List<SupplierEntity>> GetSuppliersByIdsAsync(List<Guid> suppliersIds)
        {
            var supplierEntities = await _context.Suppliers
                .Where(s => suppliersIds.Contains(s.Id)).ToListAsync();

            return supplierEntities;
        }

        public async Task<Guid> UpdateSupplierAsync(SupplierDto supplier)
        {
            await _context.Suppliers
                .Where(s => s.Id == supplier.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.SupplierName, supplier.SupplierName)
                 );

            return supplier.Id;
        }

        public async Task DeleteSuppliersAsync(List<Guid> ids)
        {
            var supplierEntities = await _context.Suppliers
                .Include(s => s.Products)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            foreach (var supplierEntity in supplierEntities)
            {
                supplierEntity.Products.Clear();
            }

            await _context.SaveChangesAsync();

            await _context.Suppliers
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
