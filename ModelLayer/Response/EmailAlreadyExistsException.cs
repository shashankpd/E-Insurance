﻿using System.Runtime.Serialization;

namespace E_Insurance.Controllers
{
    [Serializable]
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException()
        {
        }

        public EmailAlreadyExistsException(string? message) : base(message)
        {
        }

        public EmailAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmailAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}