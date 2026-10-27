using FinancialManager.Communication.Responses;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinancialManager.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is FinancialManagerException financialManagerException)
        {
            context.HttpContext.Response.StatusCode = (int)financialManagerException.GetStatusCode();

            context.Result = new ObjectResult(new ResponseErrorJson(financialManagerException.GetErrorMessages()));
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}