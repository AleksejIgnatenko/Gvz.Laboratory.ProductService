using Gvz.Laboratory.ProductService.Entities;

namespace Gvz.Laboratory.ProductService.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}