namespace Gvz.Laboratory.ProductService.Contracts
{
    public record GetProductsForOptionsResponse(
        Guid Id,
        string ProductName,
        List<Guid> SuppliersIds
        );
}
