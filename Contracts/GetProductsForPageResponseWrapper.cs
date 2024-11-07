namespace Gvz.Laboratory.ProductService.Contracts
{
    public record GetProductsForPageResponseWrapper(
        List<GetProductsResponse> Products,
        int NumberProducts
        );
}
