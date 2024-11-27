namespace Gvz.Laboratory.ProductService.Contracts
{
    public record UpdateProductRequest(
        Guid Id,
        string ProductName,
        string UnitsOfMeasurement,
        List<Guid> SuppliersIds
        );
}
