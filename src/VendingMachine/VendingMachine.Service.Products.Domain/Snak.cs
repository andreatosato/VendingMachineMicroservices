namespace VendingMachine.Service.Products.Domain
{
    public class Snak : Product
    {
        public decimal Grams { get; }

        public Snak(string name, GrossPrice price, decimal grams) 
            : base(name, price)
        {
            Grams = grams;
        }
    }
}
