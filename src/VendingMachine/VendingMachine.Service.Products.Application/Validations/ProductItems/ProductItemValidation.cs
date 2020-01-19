using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Products.Application.Validations.Products;
using VendingMachine.Service.Products.Application.ViewModels.ProductItems;
using VendingMachine.Service.Products.Application.ViewModels.Products;

namespace VendingMachine.Service.Products.Application.Validations.ProductItems
{
    public class ProductItemValidation<T> : AbstractValidator<ProductItemViewModel<T>>
        where T : ProductViewModel
    {
        public ProductItemValidation()
        {
            When(vm => vm is ColdDrinkViewModel, () =>
            {
                RuleFor(x => x.Product as ColdDrinkViewModel)
                    .SetValidator<IValidator<ColdDrinkViewModel>>(t => new ColdDrinkValidation());
            });

            When(vm => vm is HotDrinkViewModel, () =>
            {
                RuleFor(x => x.Product as HotDrinkViewModel)
                    .SetValidator<IValidator<HotDrinkViewModel>>(t => new HotDrinkValidation());
            });

            When(vm => vm is SnackViewModel, () =>
            {
                RuleFor(x => x.Product as SnackViewModel)
                    .SetValidator<IValidator<SnackViewModel>>(t => new SnackValidation());
            });
        }
    }
}
