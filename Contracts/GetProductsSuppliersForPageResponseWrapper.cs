namespace Gvz.Laboratory.ProductService.Contracts
{
    public record GetProductsSuppliersForPageResponseWrapper(
        List<GetSuppliersResponse> Suppliers,
        int numberSuppliers
        );
}
