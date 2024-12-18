﻿using Gvz.Laboratory.ProductService.Abstractions;
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
                UnitsOfMeasurement = product.UnitsOfMeasurement,
                Suppliers = existingSuppliers,
                DateCreate = DateTime.UtcNow,
            };

            await _context.Products.AddAsync(productEntity);
            await _context.SaveChangesAsync();
            return productEntity.Id;
        }

        public async Task<ProductEntity?> GetProductEntityByIdAsync(Guid productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            var productEntities = await _context.Products
                .AsNoTracking()
                .Include(p => p.Suppliers)
                .OrderByDescending(p => p.DateCreate)
                .ToListAsync();

            var products = productEntities.Select(p => ProductModel.Create(
                p.Id,
                p.ProductName,
                p.UnitsOfMeasurement,
                p.Suppliers.Select(s => SupplierModel.Create(s.Id, s.SupplierName)).ToList(),
                false).product).ToList();

            return products;
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
                p.UnitsOfMeasurement,
                false).product).ToList();

            return (products, numberProducts);
        }

        public async Task<(List<ProductModel> products, int numberProducts)> SearchProductsAsync(string searchQuery, int pageNumber)
        {
            var productEntities = await _context.Products
                    .AsNoTracking()
                    .Where(p => 
                        p.ProductName.ToLower().Contains(searchQuery.ToLower()) ||
                        p.UnitsOfMeasurement.ToLower().Contains(searchQuery.ToLower()) 
                    )
                    .OrderByDescending(p => p.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            var numberProducts = await _context.Products
                    .AsNoTracking()
                    .CountAsync(p => p.ProductName.ToLower().Contains(searchQuery.ToLower()));

            var products = productEntities.Select(p => ProductModel.Create(
                p.Id,
                p.ProductName,
                p.UnitsOfMeasurement,
                false).product).ToList();

            return (products, numberProducts);
        }

        public async Task<Guid> UpdateProductAsync(ProductModel product, List<Guid> supplierIds)
        {
            var supplierEntities = await _supplierRepository.GetSuppliersByIdsAsync(supplierIds)
                ?? throw new RepositoryException("Поставщик(и) не был(и) найден(ы)");

            var existingProductEntity = await _context.Products
                .Include(p => p.Suppliers)
                .FirstOrDefaultAsync(p => p.Id == product.Id)
                ?? throw new RepositoryException("Продукт не найден");

            var existingProductName = await _context.Products
                .FirstOrDefaultAsync(p => (p.ProductName == product.ProductName) && (p.ProductName != existingProductEntity.ProductName));
            if (existingProductName != null)
            {
                throw new RepositoryException("Продукт с таким названием уже существует.");
            }

            if ((supplierEntities != null) && (existingProductEntity != null))
            {
                existingProductEntity.ProductName = product.ProductName;
                existingProductEntity.UnitsOfMeasurement = product.UnitsOfMeasurement;

                existingProductEntity.Suppliers.Clear();
                existingProductEntity.Suppliers.AddRange(supplierEntities);

                await _context.SaveChangesAsync();
            }

            return product.Id;
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
