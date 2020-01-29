using FluentValidation;
using VendingMachine.Service.Orders.Application.ViewModels;

namespace VendingMachine.Service.Orders.Application.Validations
{
    public class OrderValidation : AbstractValidator<OrderAddViewModel>
    {
        public OrderValidation()
        {
            RuleFor(t => t.MachineId).GreaterThanOrEqualTo(0).NotEmpty();
            RuleForEach(t => t.ProductItems).GreaterThanOrEqualTo(0).NotEmpty();
        }
    }
}
