using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Orders.Application.ViewModels;
using VendingMachine.Service.Products.ServiceCommunications.Client.Services;

namespace VendingMachine.Service.Orders.Application.Validations
{
    public class OrderProductItemValidation : AbstractValidator<OrderProductItemViewModel>
    {
        private readonly IProductItemClientService productItemClient;

        public OrderProductItemValidation(IProductItemClientService productItemClient)
        {
            RuleFor(x => x.ProductItem).GreaterThanOrEqualTo(0).MustAsync(CheckProductItemExist);
            this.productItemClient = productItemClient;
        }

        public async Task<bool> CheckProductItemExist(int productItemId, CancellationToken token)
        {
            return await productItemClient.ExistProductItemAsync(productItemId).ConfigureAwait(false);
        }
    }

    public class OrderUpdateProductItemValidation : AbstractValidator<OrderUpdateProductItemViewModel>
    {
        public OrderUpdateProductItemValidation()
        {
            RuleFor(t => t.OrderId).NotEmpty().GreaterThan(0);
            RuleFor(t => t.ProductItem).NotEmpty().GreaterThan(0);
        }
    }

    public class OrderUpdateProductItemsValidation : AbstractValidator<OrderUpdateProductItemsViewModel>
    {
        public OrderUpdateProductItemsValidation()
        {
            RuleFor(t => t.OrderId).NotEmpty().GreaterThan(0);
            RuleForEach(t => t.ProductItems).NotEmpty().GreaterThan(0);
        }
    }
}
