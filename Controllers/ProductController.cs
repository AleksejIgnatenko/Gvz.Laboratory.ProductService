using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Contracts;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gvz.Laboratory.ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductAsync([FromBody] CreateProductRequest createProductRequest)
        {
            var id = await _productService.CreateProductAsync(Guid.NewGuid(),
                                                        createProductRequest.ProductName,
                                                        createProductRequest.SupplierIds);
            return Ok();
        }

        [HttpGet]
        [Route("GetProductsForPage")]
        public async Task<ActionResult> GetProductsForPageAsync(int pageNumber)
        {
            var (products, numberProducts) = await _productService.GetProductsForPageAsync(pageNumber);

            var response = products.Select(p => new GetProductsForPageResponse(p.Id, p.ProductName)).ToList();

            var responseWrapper = new GetProductsForPageResponseWrapper(response, numberProducts);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("GetSuppliersForProductPageAsync")]
        public async Task<ActionResult> GetSuppliersForProductPageAsync(Guid productId,int pageNumber)
        {
            var (products, numberSuppliers) = await _productService.GetSuppliersForProductPageAsync(productId, pageNumber);

            var response = products.Select(p => new GetSuppliersForPageResponse(p.Id, p.SupplierName, p.Manufacturer)).ToList();

            var responseWrapper = new GetSuppliersForPageResponseWrapper(response, numberSuppliers);

            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateProductAsync(Guid id, [FromBody] UpdateProductRequest updateProductRequest)
        {
            await _productService.UpdateProductAsync(id, updateProductRequest.ProductName);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProductAsync([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No supplier IDs provided.");
            }

            await _productService.DeleteProductAsync(ids);

            return Ok();
        }
    }
}
