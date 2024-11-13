using Gvz.Laboratory.ProductService.Dto;
using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Abstractions
{
    public interface IPartyRepository
    {
        Task<Guid> CreatePartyAsync(PartyDto party);
        Task DeletePartiesAsync(List<Guid> ids);
        Task<(List<PartyModel> parties, int numberParties)> GetProductPartiesForPageAsync(Guid productId, int pageNumber);
        Task<Guid> UpdatePartyAsync(PartyDto party);
    }
}