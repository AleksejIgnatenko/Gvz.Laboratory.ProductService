using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Dto;
using Gvz.Laboratory.ProductService.Exceptions;
using Gvz.Laboratory.ProductService.Models;
using OfficeOpenXml;

namespace Gvz.Laboratory.ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductKafkaProducer _productKafkaProducer;

        public ProductService(IProductRepository productRepository, IProductKafkaProducer productKafkaProducer)
        {
            _productRepository = productRepository;
            _productKafkaProducer = productKafkaProducer;
        }

        public async Task<Guid> CreateProductAsync(Guid id, string name, List<Guid> supplierIds)
        {
            var (errors, product) = ProductModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ProductValidationException(errors);
            }

            var productId = await _productRepository.CreateProductAsync(product, supplierIds);

            ProductDto productDto = new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                SupplierIds = supplierIds
            };

            await _productKafkaProducer.SendToKafkaAsync(productDto, "add-product-topic");

            return productId;
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            return await _productRepository.GetProductsAsync();
        }

        public async Task<(List<ProductModel> products, int numberProducts)> GetProductsForPageAsync(int pageNumber)
        {
            return await _productRepository.GetProductsForPageAsync(pageNumber);
        }

        public async Task<(List<ProductModel> products, int numberProducts)> SearchProductsAsync(string searchQuery, int pageNumber)
        {
            return await _productRepository.SearchProductsAsync(searchQuery, pageNumber);
        }

        public async Task<MemoryStream> ExportProductsToExcelAsync()
        {
            var manufacturers = await _productRepository.GetProductsAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Manufacturers");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Название";

                for (int i = 0; i < manufacturers.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = manufacturers[i].Id;
                    worksheet.Cells[i + 2, 2].Value = manufacturers[i].ProductName;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);

                stream.Position = 0; // Сбрасываем поток
                return stream;
            }
        }


        public async Task<Guid> UpdateProductAsync(Guid id, string name, List<Guid> supplierIds)
        {
            var (errors, product) = ProductModel.Create(id, name);
            if (errors.Count > 0)
            {
                throw new ProductValidationException(errors);
            }

            var productId = await _productRepository.UpdateProductAsync(product, supplierIds);

            ProductDto productDto = new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                SupplierIds = supplierIds,
            };
            await _productKafkaProducer.SendToKafkaAsync(productDto, "update-product-topic");

            return productId;
        }

        public async Task DeleteProductAsync(List<Guid> ids)
        {
            await _productRepository.DeleteProductsAsync(ids);
            await _productKafkaProducer.SendToKafkaAsync(ids, "delete-product-topic");
        }
    }
}
