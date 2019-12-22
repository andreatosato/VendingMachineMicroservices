using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Service.Shared.Infrastructure
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}
