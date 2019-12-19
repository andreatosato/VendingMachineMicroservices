using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Services.Shared.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}
