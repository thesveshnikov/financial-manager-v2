using System.Net;

namespace FinancialManager.Exception.ExceptionsBase;

public class ErrorOnValidationException : FinancialManagerException
{
    private readonly List<string> _errors;

    public ErrorOnValidationException(List<string> errorMessages)
    {
        _errors = errorMessages;
    }
    
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;

    public override List<string> GetErrorMessages() => _errors;
}