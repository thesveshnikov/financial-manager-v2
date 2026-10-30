using System.Net;

namespace FinancialManager.Exception.ExceptionsBase;

public class RefreshTokenExpiredException : FinancialManagerException
{
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;

    public override List<string> GetErrorMessages() => [ResourceMessagesException.EXPIRED_REFRESH_TOKEN];
}