using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Contracts;
using Microsoft.AspNetCore.Mvc;

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
                                                        createProductRequest.SuppliersIds);
            return Ok();
        }

        [HttpGet]
        [Route("getProductsAsync")]
        public async Task<ActionResult> GetProductsAsync()
        {
            var products = await _productService.GetProductsAsync();

            var response = products.Select(p => new GetProductsResponse(p.Id, p.ProductName)).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("getProductsForOptions")]
        public async Task<ActionResult> GetProductsForOptionsAsync()
        {
            var products = await _productService.GetProductsAsync();
            
            var response = products.Select(p => new GetProductsForOptionsResponse(p.Id, p.ProductName, p.Suppliers.Select(s => s.Id).ToList())).ToList();

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetProductsForPageAsync(int pageNumber)
        {
            var (products, numberProducts) = await _productService.GetProductsForPageAsync(pageNumber);

            var response = products.Select(p => new GetProductsResponse(p.Id, p.ProductName)).ToList();

            var responseWrapper = new GetProductsForPageResponseWrapper(response, numberProducts);

            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateProductAsync(Guid id, [FromBody] UpdateProductRequest updateProductRequest)
        {
            await _productService.UpdateProductAsync(id, updateProductRequest.ProductName, updateProductRequest.SuppliersIds);
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
