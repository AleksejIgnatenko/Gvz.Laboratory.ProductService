﻿using FluentValidation;
using Gvz.Laboratory.ProductService.Models;

namespace Gvz.Laboratory.ProductService.Validations
{
    public class ProductValidation : AbstractValidator<ProductModel>
    {
        public ProductValidation()
        {
            RuleFor(x => x.ProductName)
                    .NotEmpty().WithMessage("Название продукта не может быть пустым");

            RuleFor(x => x.UnitsOfMeasurement)
                    .NotEmpty().WithMessage("Единицы  измерения не могут быть пустыми");
        }
    }
}
