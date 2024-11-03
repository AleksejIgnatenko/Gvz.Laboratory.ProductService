namespace Gvz.Laboratory.ProductService.Entities
{
    public class SupplierEntity
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
