namespace Gvz.Laboratory.ProductService.Contracts
{
    public record GetPartiesForPageResponseWrapper(
            List<GetPartiesResponse> Parties,
            int numberParties
            );
}
