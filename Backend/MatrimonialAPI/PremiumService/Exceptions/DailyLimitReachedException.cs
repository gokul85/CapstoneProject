using System.Runtime.Serialization;

namespace PremiumService.Exceptions
{
    [Serializable]
    public class DailyLimitReachedException : Exception
    {
        public DailyLimitReachedException(string? message) : base(message)
        {
        }
    }
}