using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;
        public PartyController(IPartyService partyService)
        {
            _partyService = partyService;
        }
        [HttpGet]
        [Authorize]
        [Route("getProductPartiesForPage")]
        public async Task<ActionResult> GetProductPartiesForPageAsync(Guid productId, int pageNumber)
        {
            var (parties, numberParties) = await _partyService.GetProductPartiesForPageAsync(productId, pageNumber);
            var response = parties.Select(p => new GetPartiesResponse(p.Id,
                p.BatchNumber,
                p.DateOfManufacture,
                p.Product.ProductName,
                p.SupplierName,
                p.ManufacturerName,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                p.Surname,
                p.Note)).ToList();

            var responseWrapper = new GetPartiesForPageResponseWrapper(response, numberParties);

            return Ok(responseWrapper);
        }
    }
}
