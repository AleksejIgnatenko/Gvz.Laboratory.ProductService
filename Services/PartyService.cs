using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        public PartyService(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }
        public async Task<(List<PartyModel> parties, int numberParties)> GetProductPartiesForPageAsync(Guid productId, int pageNumber)
        {
            return await _partyRepository.GetProductPartiesForPageAsync(productId, pageNumber);
        }
    }
}
