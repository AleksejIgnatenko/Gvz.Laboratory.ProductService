namespace Gvz.Laboratory.ProductService.Contracts
{
    public record UpdateProductRequest(
        Guid Id,
        string ProductName,
        List<Guid> SuppliersIds
        );
}
