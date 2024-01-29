using System;
namespace PettyCashNeve_ServerSide.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
