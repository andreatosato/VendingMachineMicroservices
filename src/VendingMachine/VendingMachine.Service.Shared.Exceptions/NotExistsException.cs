using System;

namespace VendingMachine.Service.Shared.Exceptions
{
    public class NotExistsException : Exception
    {
        public int NotFoundId { get; set; }
        public NotExistsException(int id)
            : base()
        {
            NotFoundId = id;
        }

        public NotExistsException(string message, int id)
            : base(message)
        {
            NotFoundId = id;
        }

        public NotExistsException(string message, Exception innerException, int id)
            : base(message, innerException)
        {
            NotFoundId = id;
        }
    }
}
