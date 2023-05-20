namespace TestWebAPI.Exceptions;

// Власне виключення для швидкого повернення по стеку викликів
public class BusinessException : Exception
{
    public BusinessException()
    {
    }

    public BusinessException(string message)
        : base(message)
    {
    }

    public BusinessException(string message, Exception ex)
        : base(message, ex)
    {
    }
}
