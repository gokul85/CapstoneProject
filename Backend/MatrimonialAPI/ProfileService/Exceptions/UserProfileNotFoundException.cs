using System.Runtime.Serialization;

namespace ProfileService.Exceptions
{
    [Serializable]
    public class UserProfileNotFoundException : Exception
    {
        public UserProfileNotFoundException(string? message) : base(message)
        {
        }
    }
}