using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Workers.ImporterProducts.Readers
{
    public interface IProductItemsImporter
    {
        Task DoWorkAsync(string Name, string FullName);
    }
}
