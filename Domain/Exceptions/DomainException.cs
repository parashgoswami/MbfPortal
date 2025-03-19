namespace Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException()
    {
    }
    public DomainException(string? message) : base(message)
    {
    }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class InvalidAmountException : DomainException
{
    public InvalidAmountException(string paramName)
        : base($"Amount cannot be negative: {paramName}") { }
}