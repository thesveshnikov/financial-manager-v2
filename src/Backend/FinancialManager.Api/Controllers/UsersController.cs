using System.Threading.Tasks;
using FinancialManager.Application.UseCases.User.ChangePassword;
using FinancialManager.Application.UseCases.User.Profile;
using FinancialManager.Application.UseCases.User.Register;
using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }
    
    [HttpPut("password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePassword(
        [FromServices] IChangePasswordUseCase useCase,
        [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}