namespace VendingMachine.Service.Products.Domain
{
    public class Snack : Product
    {
        public decimal Grams { get; }

        public Snack(string name, GrossPrice price, decimal grams) 
            : base(name, price)
        {
            Grams = grams;
        }
    }
}
