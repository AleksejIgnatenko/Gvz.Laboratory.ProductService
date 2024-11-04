namespace Gvz.Laboratory.ProductService.Contracts
{
    public record CreateProductRequest(
        string ProductName,
        List<Guid> SuppliersIds
        );
}
