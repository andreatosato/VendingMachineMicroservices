using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Machines.ServiceCommunications.Client.Services;
using VendingMachine.Service.Orders.Application.ViewModels;
using VendingMachine.Service.Products.ServiceCommunications.Client.Services;

namespace VendingMachine.Service.Orders.Application.Validations
{
    public class OrderValidation : AbstractValidator<OrderAddViewModel>
    {
        public OrderValidation(IMachineClientService machineClient, IProductItemClientService productItemClient)
        {
            RuleFor(t => t.ModelStatus).SetValidator(new MachineStatusValidation(machineClient));
            RuleForEach(t => t.ProductItems).SetValidator(new OrderProductItemValidation(productItemClient));
        }
    }
}
