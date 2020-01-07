using FluentValidation;
using VendingMachine.Service.Machines.Application.ViewModels;

namespace VendingMachine.Service.Machines.Application.Validations.Machines
{
    class MachineItemValidation
    {
    }

    public class BuyProductsValidation : AbstractValidator<BuyProductsViewModel>
    {
        public BuyProductsValidation()
        {
            RuleFor(x => x.TotalBuy).NotEmpty().GreaterThan(0);
            RuleFor(t => t.TotalRest).NotEmpty().GreaterThanOrEqualTo(0);

            RuleForEach(t => t.Products).GreaterThan(0);
        }
    }
}
