using System.Runtime.Serialization;

namespace PremiumService.Exceptions
{
    [Serializable]
    public class UnableToRetriveContactDetails : Exception
    {
        public UnableToRetriveContactDetails(string? message) : base(message)
        {
        }
    }
}