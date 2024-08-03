using System.Runtime.Serialization;

namespace AuthService.Exceptions
{
    [Serializable]
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException(string? message) : base(message)
        {
        }
    }
}