using System.Runtime.Serialization;

namespace AuthService.Exceptions
{
    [Serializable]
    public class NoUserFoundException : Exception
    {
        public NoUserFoundException(string? message) : base(message)
        {
        }
    }
}