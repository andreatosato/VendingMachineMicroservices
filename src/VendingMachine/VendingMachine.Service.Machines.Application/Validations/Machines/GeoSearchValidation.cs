using FluentValidation;
using VendingMachine.Service.Machines.Application.ViewModels;

namespace VendingMachine.Service.Machines.Application.Validations.Machines
{
    public class GeoSearchValidation : AbstractValidator<GeoSearchViewModel>
    {
        public GeoSearchValidation()
        {
            RuleFor(x => x.Latutide).NotEmpty().GreaterThanOrEqualTo(-90).LessThanOrEqualTo(90);
            RuleFor(x => x.Longitude).NotEmpty().GreaterThanOrEqualTo(-180).LessThanOrEqualTo(180);
            RuleFor(x => x.Radius).GreaterThanOrEqualTo(0);
        }
    }
}
