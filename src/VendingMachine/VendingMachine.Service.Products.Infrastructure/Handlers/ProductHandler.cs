using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Products.Infrastructure.Commands;

namespace VendingMachine.Service.Products.Infrastructure.Handlers
{
    public class ProductHandler : 
        IRequestHandler<ColdDrinkAddCommand, Unit>,
        IRequestHandler<HotDrinkAddCommand, Unit>,
        IRequestHandler<SnackAddCommand, Unit>,
        IRequestHandler<ProductDeleteCommand, Unit>
    {
        private readonly IMediator mediator;
        private readonly IProductsUoW productUoW;
        private readonly ILogger logger;

        public ProductHandler(IMediator mediator, IProductsUoW productUoW, ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.productUoW = productUoW;
            this.logger = loggerFactory.CreateLogger(typeof(ProductHandler));
        }

        public async Task<Unit> Handle(ColdDrinkAddCommand request, CancellationToken cancellationToken)
        {
            var grossPrice = new GrossPrice(request.Price.GrossPrice, request.Price.TaxPercentage);
            var domain = new ColdDrink(request.Name, grossPrice, request.Litre);
            domain.SetTemperatureMaximun(request.TemperatureMaximum);
            domain.SetTemperatureMinimum(request.TemperatureMinimum);

            await productUoW.ProductRepository.AddAsync(domain);
            await productUoW.SaveAsync();
            return new Unit();
        }

        public async Task<Unit> Handle(HotDrinkAddCommand request, CancellationToken cancellationToken)
        {
            var grossPrice = new GrossPrice(request.Price.GrossPrice, request.Price.TaxPercentage);
            var domain = new HotDrink(request.Name, grossPrice, request.Grams);
            domain.SetTemperatureMaximun(request.TemperatureMaximum);
            domain.SetTemperatureMinimum(request.TemperatureMinimum);

            await productUoW.ProductRepository.AddAsync(domain);
            await productUoW.SaveAsync();
            return new Unit();
        }

        public async Task<Unit> Handle(SnackAddCommand request, CancellationToken cancellationToken)
        {
            var grossPrice = new GrossPrice(request.Price.GrossPrice, request.Price.TaxPercentage);
            var domain = new Snack(request.Name, grossPrice, request.Grams);

            await productUoW.ProductRepository.AddAsync(domain);
            await productUoW.SaveAsync();
            return new Unit();
        }

        public async Task<Unit> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var domain = await productUoW.ProductRepository.FindAsync(request.ProductId);
            await productUoW.ProductRepository.DeleteAsync(domain);
            await productUoW.SaveAsync();
            return new Unit();
        }

    }
}
