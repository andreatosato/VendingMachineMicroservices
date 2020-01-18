using FluentValidation;
using VendingMachine.Service.Products.Application.ViewModels;

namespace VendingMachine.Service.Products.Application.Validations.Products
{
    public class SnackValidation : AbstractValidator<SnackViewModel>
    {
        public SnackValidation()
        {
        }
    }
}
