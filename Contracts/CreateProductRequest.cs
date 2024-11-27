namespace Gvz.Laboratory.ProductService.Contracts
{
    public record CreateProductRequest(
        string ProductName,
        string UnitsOfMeasurement,
        List<Guid> SuppliersIds
        );
}
