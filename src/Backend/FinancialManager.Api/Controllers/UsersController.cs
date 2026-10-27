using System.Threading.Tasks;
using FinancialManager.Application.UseCases.User.Register;
using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserAccountUseCase useCase,
        [FromBody] RequestRegisterUserAccountJson request)
    {
        var response = await useCase.Execute(request);
        
        return Created(string.Empty, response);
    }
}