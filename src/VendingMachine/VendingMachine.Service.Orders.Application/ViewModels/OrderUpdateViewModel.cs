namespace VendingMachine.Service.Orders.Application.ViewModels
{
    public class OrderUpdateViewModel : OrderAddViewModel
    {
        public int OrderId { get; set; }
    }

    public class OrderUpdatedViewModel : OrderAddedViewModel
    {
    }
}
