using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISuppliersService _suppliersService;

        public SupplierController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        [HttpGet]
        [Route("getProductSuppliers")]
        [Authorize]
        public async Task<ActionResult> GetProductSuppliersAsync(Guid productId)
        {
            var suppliers = await _suppliersService.GetProductSuppliersAsync(productId);

            var response = suppliers.Select(s => new GetSuppliersResponse(s.Id, s.SupplierName)).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("getProductSuppliersForPageAsync")]
        [Authorize]
        public async Task<ActionResult> GetProductSuppliersForPageAsync(Guid productId, int pageNumber)
        {
            var (suppliers, numberSuppliers) = await _suppliersService.GetProductSuppliersForPageAsync(productId, pageNumber);

            var response = suppliers.Select(s => new GetSuppliersResponse(s.Id, s.SupplierName)).ToList();

            var responseWrapper = new GetProductsSuppliersForPageResponseWrapper(response, numberSuppliers);

            return Ok(responseWrapper);
        }
    }
}
