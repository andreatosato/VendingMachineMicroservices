using FluentValidation;
using VendingMachine.Service.Machines.Application.ViewModels;

namespace VendingMachine.Service.Machines.Application.Validations.Coins
{
    public class AddCoinsValidation : AbstractValidator<AddCoinsViewModel>
    {
        //public AddCoinsValidation()
        //{
        //    RuleFor(t => t.Coins).NotEmpty().GreaterThan(0);
        //    RuleFor(t => t.MachineId).NotEmpty().GreaterThan(0);
        //}
    }

    public class CollectCoinsValidation : AbstractValidator<CollectCoinsViewModel>
    {
        public CollectCoinsValidation()
        {
            RuleFor(t => t.MachineId).NotEmpty().GreaterThan(0);
        }
    }

    public class RequestRestValidation : AbstractValidator<RequestRestViewModel>
    {
        public RequestRestValidation()
        {
            RuleFor(t => t.MachineId).NotEmpty().GreaterThan(0);
            RuleFor(t => t.Rest).NotEmpty().GreaterThan(0);
        }
    }
}
