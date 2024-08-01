using System.Runtime.Serialization;

namespace PremiumService.Exceptions
{
    [Serializable]
    internal class UnableToRetriveContactDetails : Exception
    {
        public UnableToRetriveContactDetails()
        {
        }

        public UnableToRetriveContactDetails(string? message) : base(message)
        {
        }

        public UnableToRetriveContactDetails(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnableToRetriveContactDetails(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}