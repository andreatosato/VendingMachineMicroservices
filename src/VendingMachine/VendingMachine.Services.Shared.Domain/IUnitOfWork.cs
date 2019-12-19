using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Services.Shared.Domain
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();
    }
}
