namespace Gvz.Laboratory.ProductService.Contracts
{
    public record GetSuppliersForPageResponseWrapper(
        List<GetSuppliersForPageResponse> Suppliers,
        int NumberSuppliers
        );
}
