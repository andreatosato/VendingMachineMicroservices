using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Shared.Read
{
    public interface IPagedRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public sealed class PagedRequest : IPagedRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
