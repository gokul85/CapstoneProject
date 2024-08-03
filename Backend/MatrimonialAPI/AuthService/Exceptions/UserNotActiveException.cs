using System.Runtime.Serialization;

namespace AuthService.Exceptions
{
    [Serializable]
    public class UserNotActiveException : Exception
    {
        public UserNotActiveException(string? message) : base(message)
        {
        }
    }
}