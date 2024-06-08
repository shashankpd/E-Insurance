using System.Runtime.Serialization;

namespace E_Insurance.Controllers
{
    [Serializable]
    public class PolicyAlreadyExistsException : Exception
    {
        public PolicyAlreadyExistsException()
        {
        }

        public PolicyAlreadyExistsException(string? message) : base(message)
        {
        }

        public PolicyAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PolicyAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}