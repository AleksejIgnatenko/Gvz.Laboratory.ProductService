using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Contracts;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,Manager,Worker")]
        public async Task<ActionResult> CreateProductAsync([FromBody] CreateProductRequest createProductRequest)
        {
            var id = await _productService.CreateProductAsync(Guid.NewGuid(),
                                                        createProductRequest.ProductName,
                                                        createProductRequest.UnitsOfMeasurement,
                                                        createProductRequest.SuppliersIds);
            return Ok();
        }

        [HttpGet]
        [Route("getProductsAsync")]
        [Authorize]
        public async Task<ActionResult> GetProductsAsync()
        {
            var products = await _productService.GetProductsAsync();

            var response = products.Select(p => new GetProductsResponse(p.Id, p.ProductName, p.UnitsOfMeasurement)).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("getProductsForOptions")]
        [Authorize]
        public async Task<ActionResult> GetProductsForOptionsAsync()
        {
            var products = await _productService.GetProductsAsync();
            
            var response = products.Select(p => new GetProductsForOptionsResponse(p.Id, p.ProductName, p.Suppliers.Select(s => s.Id).ToList())).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetProductsForPageAsync(int pageNumber)
        {
            var (products, numberProducts) = await _productService.GetProductsForPageAsync(pageNumber);

            var response = products.Select(p => new GetProductsResponse(p.Id, p.ProductName, p.UnitsOfMeasurement)).ToList();

            var responseWrapper = new GetProductsForPageResponseWrapper(response, numberProducts);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("exportProductsToExcel")]
        [Authorize]
        public async Task<ActionResult> ExportProductsToExcelAsync()
        {
            var stream = await _productService.ExportProductsToExcelAsync();
            var fileName = "Products.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        [Route("searchProducts")]
        [Authorize]
        public async Task<ActionResult> SearchProductsAsync(string searchQuery, int pageNumber)
        {
            var (products, numberProducts) = await _productService.SearchProductsAsync(searchQuery, pageNumber);
            var response = products.Select(p => new GetProductsResponse(p.Id, p.ProductName, p.UnitsOfMeasurement)).ToList();
            var responseWrapper = new GetProductsForPageResponseWrapper(response, numberProducts);
            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Manager,Worker")]
        public async Task<ActionResult> UpdateProductAsync(Guid id, [FromBody] UpdateProductRequest updateProductRequest)
        {
            await _productService.UpdateProductAsync(id, updateProductRequest.ProductName, updateProductRequest.UnitsOfMeasurement, updateProductRequest.SuppliersIds);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Manager,Worker")]
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
