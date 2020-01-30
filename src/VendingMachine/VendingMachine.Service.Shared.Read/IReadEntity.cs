using System.Collections.Generic;

namespace VendingMachine.Service.Shared.Read
{
    public interface IReadEntity
    {
    }

    public interface IReadPagedEntity<T>
        where T : IReadEntity
    {
        public ICollection<T> Entities { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
