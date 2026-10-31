using System.Net;

namespace FinancialManager.Exception.ExceptionsBase;

public class NotFoundException : FinancialManagerException
{
    private readonly string _message;

    public NotFoundException(string message)
    {
        _message = message;
    }

    public override List<string> GetErrorMessages() => [_message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}