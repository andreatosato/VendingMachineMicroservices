using FluentValidation;
using FluentValidation.Results;
using VendingMachine.Service.Machines.Application.ViewModels;
using VendingMachine.Service.Machines.Read;

namespace VendingMachine.Service.Machines.Application.Validations.Machines
{
    public class BuyProductsValidation : AbstractValidator<BuyProductsViewModel>
    {
        public BuyProductsValidation(IMachineQuery query)
        {
            RuleFor(t => t.MachineId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.TotalBuy).NotEmpty().GreaterThan(0);
            RuleFor(t => t.TotalRest).GreaterThanOrEqualTo(0);

            RuleForEach(t => t.Products).GreaterThan(0);
            var producsts = query.GetProductsInMachineAsync(1).ConfigureAwait(false).GetAwaiter().GetResult();
            RuleForEach(t => t.Products).Custom((p, ctx) => 
            {
                if (producsts.Products.Find(x => x.Id == (int)ctx.PropertyValue) == null)
                    ctx.AddFailure(new ValidationFailure("Products", $"There isn't a product with id {(int)ctx.PropertyValue}. Error is in {ctx.PropertyName}"));
            });
        }
    }

    public class LoadProductsValidation : AbstractValidator<LoadProductsViewModel>
    {
        public LoadProductsValidation(IMachineQuery query)
        {
            RuleFor(t => t.MachineId).NotEmpty().GreaterThan(0);
            RuleForEach(t => t.Products).GreaterThan(0);

            var producsts = query.GetProductsInMachineAsync(1).ConfigureAwait(false).GetAwaiter().GetResult();
            RuleForEach(t => t.Products).Custom((p, ctx) =>
            {
                if (producsts.Products.Find(x => x.Id == (int)ctx.PropertyValue) != null)
                    ctx.AddFailure(new ValidationFailure("Products", $"There is already a product with id {(int)ctx.PropertyValue}. Error is in {ctx.PropertyName}"));
            });
        }
    }
}
