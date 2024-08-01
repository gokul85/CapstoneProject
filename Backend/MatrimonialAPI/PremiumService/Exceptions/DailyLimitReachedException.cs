using System.Runtime.Serialization;

namespace PremiumService.Exceptions
{
    [Serializable]
    internal class DailyLimitReachedException : Exception
    {
        public DailyLimitReachedException()
        {
        }

        public DailyLimitReachedException(string? message) : base(message)
        {
        }

        public DailyLimitReachedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DailyLimitReachedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}