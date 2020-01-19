using MediatR;

namespace VendingMachine.Service.Products.Infrastructure.Commands
{
    public abstract class ProductAddCommand : IRequest<ProductAddedResult>
    {
        public string Name { get; set; }
        public PriceCommand Price { get; set; }
    }

    public class ColdDrinkAddCommand : ProductAddCommand
    {
        public decimal TemperatureMaximum { get; set; }
        public decimal TemperatureMinimum { get; set; }
        public decimal Litre { get; set; }
    }

    public class HotDrinkAddCommand : ProductAddCommand
    {
        public decimal TemperatureMaximum { get; set; }
        public decimal TemperatureMinimum { get; set; }
        public decimal Grams { get; set; }
    }

    public class SnackAddCommand : ProductAddCommand
    {
        public decimal Grams { get; set; }
    }

    public class ProductAddedResult
    {
        public int Id { get; set; }
    }
}
