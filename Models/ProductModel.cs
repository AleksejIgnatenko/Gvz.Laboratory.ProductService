using FluentValidation.Results;
using Gvz.Laboratory.ProductService.Validations;

namespace Gvz.Laboratory.ProductService.Models
{
    public class ProductModel
    {
        public Guid Id { get; }
        public string ProductName { get; } = string.Empty;
        public string UnitsOfMeasurement { get; } = string.Empty;
        public List<SupplierModel> Suppliers { get; } = new List<SupplierModel>();

        public ProductModel(Guid id, string productName, string unitsOfMeasurement)
        {
            Id = id;
            UnitsOfMeasurement = unitsOfMeasurement;
            ProductName = productName;
        }

        public ProductModel(Guid id, string productName, string unitsOfMeasurement, List<SupplierModel> suppliers)
        {
            Id = id;
            ProductName = productName;
            UnitsOfMeasurement = unitsOfMeasurement;
            Suppliers = suppliers;
        }

        public ProductModel()
        {
        }

        public static (Dictionary<string, string> errors, ProductModel product) Create(Guid id, string productName, string unitsOfMeasurement, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            ProductModel product = new ProductModel(id, productName, unitsOfMeasurement);
            if (!useValidation) { return (errors, product); }

            ProductValidation productValidation = new ProductValidation();
            ValidationResult validationResult = productValidation.Validate(product);
            if(!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, product);
        }

        public static (Dictionary<string, string> errors, ProductModel product) Create(Guid id, string productName, string unitsOfMeasurement, List<SupplierModel> suppliers, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            ProductModel product = new ProductModel(id, productName, unitsOfMeasurement, suppliers);
            if (!useValidation) { return (errors, product); }

            ProductValidation productValidation = new ProductValidation();
            ValidationResult validationResult = productValidation.Validate(product);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, product);
        }
    }
}
