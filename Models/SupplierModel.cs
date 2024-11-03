namespace Gvz.Laboratory.ProductService.Models
{
    public class SupplierModel
    {
        public Guid Id { get; }
        public string SupplierName { get; } = string.Empty;

        public SupplierModel(Guid id, string supplierName)
        {
            Id = id;
            SupplierName = supplierName;
        }

        public static SupplierModel Create(Guid id, string supplierName)
        {
            SupplierModel supplier = new SupplierModel(id, supplierName);
            return supplier;
        }
    }
}
