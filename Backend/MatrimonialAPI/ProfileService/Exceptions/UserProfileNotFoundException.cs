using System.Runtime.Serialization;

namespace ProfileService.Exceptions
{
    [Serializable]
    internal class UserProfileNotFoundException : Exception
    {
        public UserProfileNotFoundException()
        {
        }

        public UserProfileNotFoundException(string? message) : base(message)
        {
        }

        public UserProfileNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserProfileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}