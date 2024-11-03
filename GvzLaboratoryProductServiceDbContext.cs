using Gvz.Laboratory.ProductService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ProductService
{
    public class GvzLaboratoryProductServiceDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }

        public GvzLaboratoryProductServiceDbContext(DbContextOptions<GvzLaboratoryProductServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configuration db
        }
    }
}
