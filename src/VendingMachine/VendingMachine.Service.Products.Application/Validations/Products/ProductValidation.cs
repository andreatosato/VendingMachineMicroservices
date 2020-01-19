using FluentValidation;
using VendingMachine.Service.Products.Application.ViewModels.Products;

namespace VendingMachine.Service.Products.Application.Validations.Products
{
    public class ProductValidation : AbstractValidator<ProductViewModel>
    {
        public ProductValidation()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Price).SetValidator(new GrossPriceValidation());
        }
    }

    public class GrossPriceValidation : AbstractValidator<GrossPriceViewModel>
    {
        public GrossPriceValidation()
        {
            RuleFor(x => x.GrossPrice).GreaterThanOrEqualTo(0).NotNull();
            RuleFor(x => x.TaxPercentage).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).NotNull();
        }
    }

    public class SnackValidation : AbstractValidator<SnackViewModel>
    {
        public SnackValidation()
        {
            Include(new ProductValidation());
            RuleFor(t => t.Grams).GreaterThanOrEqualTo(0).NotNull().NotEmpty();
        }
    }

    public class ColdDrinkValidation : AbstractValidator<ColdDrinkViewModel>
    {
        public ColdDrinkValidation()
        {
            Include(new ProductValidation());
            RuleFor(t => t.TemperatureMaximum).NotNull().NotEmpty();
            RuleFor(t => t.TemperatureMinimum).NotNull().NotEmpty();
        }
    }

    public class HotDrinkValidation : AbstractValidator<HotDrinkViewModel>
    {
        public HotDrinkValidation()
        {
            Include(new ProductValidation());
            RuleFor(t => t.TemperatureMaximum).NotNull().NotEmpty();
            RuleFor(t => t.TemperatureMinimum).NotNull().NotEmpty();
            RuleFor(t => t.Grams).NotNull().NotEmpty();
        }
    }
}
