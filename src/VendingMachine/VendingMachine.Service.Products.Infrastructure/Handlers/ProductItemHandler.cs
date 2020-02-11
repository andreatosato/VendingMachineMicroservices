using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Products.Domain.DomainEvents;
using VendingMachine.Service.Products.Infrastructure.Commands;

namespace VendingMachine.Service.Products.Infrastructure.Handlers
{
    public class ProductItemHandler :
        IRequestHandler<ProductItemAddCommand, ProductItemAddResponse>,
        IRequestHandler<ProductItemDeleteCommand, Unit>,
        IRequestHandler<ProductItemPurchaseCommand, Unit>,
        IRequestHandler<ProductItemSoldCommand, Unit>,
        IRequestHandler<ProductItemRedefinePriceCommand, Unit>,
        INotificationHandler<ProductItemExpirationDateEvent>
    {
        private readonly IMediator mediator;
        private readonly IProductsUoW productUoW;
        private readonly ILogger logger;

        public ProductItemHandler(IMediator mediator, IProductsUoW productUoW, ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.productUoW = productUoW;
            this.logger = loggerFactory.CreateLogger(typeof(ProductItemHandler));
        }

        public async Task Handle(ProductItemExpirationDateEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductItemAddResponse> Handle(ProductItemAddCommand request, CancellationToken cancellationToken)
        {
            var productDomain = await productUoW.ProductRepository.FindAsync(request.ProductId);
            if (productDomain == null)
                throw new InvalidOperationException($"Product Id [{request.ProductId}] not exist");

            var domain = new ProductItem(productDomain);
            domain.SetExpirationDate(request.ExpirationDate);
            domain.SetPurchasedDate(request.Purchased);
            domain.SetGrossPriceValue(request.SoldPrice.GetValueOrDefault());

            try
            {
                var productCreated = await productUoW.ProductItemRepository.AddAsync(domain);
                await productUoW.SaveAsync();
                return new ProductItemAddResponse { ProductItemId = productCreated.Id };
            }
            catch (Exception e)
            {

                throw;
            }
            //var productCreated = await productUoW.ProductItemRepository.AddAsync(domain);
            //await productUoW.SaveAsync();
            //return new ProductItemAddResponse { ProductItemId = productCreated.Id };
        }

        public async Task<Unit> Handle(ProductItemDeleteCommand request, CancellationToken cancellationToken)
        {
            var productItemDomain = await productUoW.ProductItemRepository.FindAsync(request.ProductItemId);
            await productUoW.ProductItemRepository.DeleteAsync(productItemDomain);
            await productUoW.SaveAsync();
            return new Unit();
        }

        public async Task<Unit> Handle(ProductItemPurchaseCommand request, CancellationToken cancellationToken)
        {
            var productItemDomain = await productUoW.ProductItemRepository.FindAsync(request.ProductItemId);
            productItemDomain.SetPurchasedDate(request.PurchaseDate);
            await productUoW.ProductItemRepository.UpdateAsync(productItemDomain);
            await productUoW.SaveAsync();
            return new Unit();
        }

        public async Task<Unit> Handle(ProductItemSoldCommand request, CancellationToken cancellationToken)
        {
            var productItemDomain = await productUoW.ProductItemRepository.FindAsync(request.ProductItemId);
            productItemDomain.SetSoldDate(request.SoldDate);
            await productUoW.ProductItemRepository.UpdateAsync(productItemDomain);
            await productUoW.SaveAsync();
            return new Unit();
        }

        public async Task<Unit> Handle(ProductItemRedefinePriceCommand request, CancellationToken cancellationToken)
        {
            var productItemDomain = await productUoW.ProductItemRepository.FindAsync(request.ProductItemId);
            productItemDomain.SetGrossPriceValue(request.NewPrice);
            await productUoW.ProductItemRepository.UpdateAsync(productItemDomain);
            await productUoW.SaveAsync();
            return new Unit();
        }
    }
}
