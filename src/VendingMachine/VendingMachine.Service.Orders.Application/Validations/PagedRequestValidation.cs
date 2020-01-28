using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Orders.Application.ViewModels;

namespace VendingMachine.Service.Orders.Application.Validations
{
    public class PagedRequestValidation : AbstractValidator<PagedRequestViewModel>
    {
        public PagedRequestValidation()
        {
            RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Take).GreaterThanOrEqualTo(0);
        }
    }
}
