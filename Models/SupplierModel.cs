namespace Gvz.Laboratory.ProductService.Models
{
    public class SupplierModel
    {
        public Guid Id { get; }
        public string SupplierName { get; } = string.Empty;
        public string Manufacturer { get; } = string.Empty;

        public SupplierModel(Guid id, string name, string manufacturer)
        {
            Id = id;
            SupplierName = name;
            Manufacturer = manufacturer;
        }

        public static SupplierModel Create(Guid id, string name, string manufacturer)
        {
            SupplierModel supplier = new SupplierModel(id, name, manufacturer);
            return supplier;
        }
    }
}
