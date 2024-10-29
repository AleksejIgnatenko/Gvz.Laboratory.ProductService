namespace Gvz.Laboratory.ProductService.Contracts
{
    public record GetProductsForPageResponseWrapper(
        List<GetProductsForPageResponse> Products,
        int NumberProducts
        );
}
