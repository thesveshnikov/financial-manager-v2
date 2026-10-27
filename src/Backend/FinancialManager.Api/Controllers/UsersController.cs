using FinancialManager.Application.UseCases.User.Register;
using FinancialManager.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserAccountUseCase useCase,
        [FromBody] RequestRegisterUserAccountJson request)
    {
        var response = await useCase.Execute(request);
        
        return Created(string.Empty, response);
    }
}