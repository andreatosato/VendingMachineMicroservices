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
}
