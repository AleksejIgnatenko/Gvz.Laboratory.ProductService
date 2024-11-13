using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface IPartyService
    {
        Task<(List<PartyModel> parties, int numberParties)> GetProductPartiesForPageAsync(Guid productId, int pageNumber);
    }
}