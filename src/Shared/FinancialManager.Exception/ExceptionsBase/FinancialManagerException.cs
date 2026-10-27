using System.Net;

namespace FinancialManager.Exception.ExceptionsBase;

public abstract class FinancialManagerException: System.Exception
{
    public abstract HttpStatusCode GetStatusCode();
    public abstract List<string> GetErrorMessages();
}