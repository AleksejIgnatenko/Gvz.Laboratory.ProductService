using Gvz.Laboratory.ProductService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ProductService
{
    public class GvzLaboratoryProductServiceDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }
    }
}
