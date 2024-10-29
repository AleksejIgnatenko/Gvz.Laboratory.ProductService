namespace Gvz.Laboratory.ProductService.Contracts
{
    public record GetSuppliersForPageResponse(
        Guid Id,
        string SupplierName,
        string Manufacturer
        );
}
