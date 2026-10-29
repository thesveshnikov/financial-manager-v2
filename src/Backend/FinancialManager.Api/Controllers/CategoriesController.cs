using FinancialManager.Application.UseCases.Category.Register;
using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterCategoryUseCase useCase,
        [FromBody] RequestCategoryJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}