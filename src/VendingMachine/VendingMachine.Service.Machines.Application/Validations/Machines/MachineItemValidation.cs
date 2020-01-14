using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Application.Cachings;
using VendingMachine.Service.Machines.Application.ViewModels;
using VendingMachine.Service.Machines.Read;

namespace VendingMachine.Service.Machines.Application.Validations.Machines
{
    public class BuyProductsValidation : AbstractValidator<BuyProductsViewModel>
    {
        private readonly BulkProductValidation bulkProductValidation;

        public BuyProductsValidation(IMachineQuery query, IDistributedCache distributedCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            bulkProductValidation = new BulkProductValidation(query, distributedCache);

            RuleFor(t => t.MachineId).NotEmpty().GreaterThan(0).CustomAsync(bulkProductValidation.CheckMachineExistsAsync);
            RuleFor(x => x.TotalBuy).NotEmpty().GreaterThan(0);
            RuleFor(t => t.TotalRest).GreaterThanOrEqualTo(0);

            RuleForEach(t => t.Products).NotEmpty();

            WhenAsync((req, cancellationToken) => Task.FromResult(req.Products.Any()), () =>
            {
                RuleForEach(t => t.Products).CustomAsync(bulkProductValidation.CheckProductsInMachineExistsAsync);
            });
        }
    }

    public class LoadProductsValidation : AbstractValidator<LoadProductsViewModel>
    {
        private readonly BulkProductValidation bulkProductValidation;
        public LoadProductsValidation(IMachineQuery query, IDistributedCache distributedCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            bulkProductValidation = new BulkProductValidation(query, distributedCache);

            RuleFor(t => t.MachineId).NotEmpty().GreaterThan(0).CustomAsync(bulkProductValidation.CheckMachineExistsAsync);
            RuleForEach(t => t.Products).GreaterThan(0);

            RuleForEach(t => t.Products).CustomAsync(async (p, ctx, cancellationToken) =>
            {
                bool exist = await query.CheckHistoryProductExist(p).ConfigureAwait(false);
                if (exist)
                    ctx.AddFailure(new ValidationFailure("Products", $"There is already a product with id {p}. Error is in {ctx.PropertyName}"));
            });
        }
    }

    public class BulkProductValidation
    {
        private readonly IMachineQuery query;
        private readonly IDistributedCache distributedCache;

        public BulkProductValidation(IMachineQuery query, IDistributedCache distributedCache)
        {
            this.query = query;
            this.distributedCache = distributedCache;
        }

        public async Task CheckProductsInMachineExistsAsync(int p, CustomContext ctx, CancellationToken cancellationToken)
        {
            int machineId = ((BuyProductsViewModel)ctx.ParentContext.InstanceToValidate).MachineId;
            if (machineId > 0)
            {
                Read.Models.ProductsReadModel producsts = null;
                var productsData = await distributedCache.GetAsync(CachingKeys.ProductsInMachineKey(machineId));
                if (productsData != null)
                    producsts = await productsData.DeserializeCacheAsync<Read.Models.ProductsReadModel>();
                else
                {
                    producsts = await query.GetProductsInMachineAsync(machineId);
                    await distributedCache.SetAsync(CachingKeys.ProductsInMachineKey(machineId), producsts.SerializeCache());
                }
                if (producsts.Products.Find(x => x.Id == p) == null)
                    ctx.AddFailure(new ValidationFailure("Products", $"There isn't a product with id {p}. Error is in {ctx.PropertyName}"));
            }
        }

        public async Task CheckMachineExistsAsync(int p, CustomContext ctx, CancellationToken cancellationToken)
        {
            if (p > 0)
            {
                var machineExist = await query.CheckMachineItemExistsAsync(p);
                if (!machineExist)
                    ctx.AddFailure(new ValidationFailure("MachineId", $"There isn't a machine with id {(int)ctx.PropertyValue}. Error is in {ctx.PropertyName}"));
            }
        }
    }
}
