using System.Runtime.Serialization;

namespace AuthService.Exceptions
{
    [Serializable]
    public class UnauthorizedUserException : Exception
    {

        public UnauthorizedUserException(string? message) : base(message)
        {
        }

    }
}