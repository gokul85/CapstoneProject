using System.Runtime.Serialization;

namespace AuthService.Exceptions
{
    [Serializable]
    internal class ObjectsNotFoundException : Exception
    {
        public ObjectsNotFoundException()
        {
        }

        public ObjectsNotFoundException(string? message) : base(message)
        {
        }

        public ObjectsNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ObjectsNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}