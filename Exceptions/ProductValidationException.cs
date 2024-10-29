namespace Gvz.Laboratory.ProductService.Exceptions
{
    public class ProductValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }

        public ProductValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
