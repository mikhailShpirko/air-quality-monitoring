namespace Monitoring.Infrastructure
{
    internal class InvalidApiResponseException : Exception
    {
        public InvalidApiResponseException(string? message) : base(message)
        {
        }
    }
}
