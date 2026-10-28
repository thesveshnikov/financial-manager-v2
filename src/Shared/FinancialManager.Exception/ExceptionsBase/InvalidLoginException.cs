using System.Net;

namespace FinancialManager.Exception.ExceptionsBase;

public class InvalidLoginException : FinancialManagerException
{
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;

    public override List<string> GetErrorMessages() => [ResourceMessagesException.VALIDATION_LOGIN_INVALID];
}