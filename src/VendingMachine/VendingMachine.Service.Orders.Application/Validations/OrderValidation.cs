using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Orders.Application.ViewModels;

namespace VendingMachine.Service.Orders.Application.Validations
{
    public class OrderValidation : AbstractValidator<OrderAddViewModel>
    {
        public OrderValidation()
        {
        }
    }
}
