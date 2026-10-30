using FinancialManager.Application.UseCases.Expense.GetAll;
using FinancialManager.Application.UseCases.Expense.GetById;
using FinancialManager.Application.UseCases.Expense.Register;
using FinancialManager.Application.UseCases.Expense.UpdateById;
using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredExpenseJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Execute(
        [FromServices] IRegisterExpenseUseCase useCase,
        [FromBody] RequestExpenseJson request)
    {
        var response = await useCase.Execute(request);
        
        return Created(string.Empty, response);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseExpensesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllExpensesUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseExpenseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetExpenseByIdUseCase useCase,
        [FromRoute] Guid id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateExpenseByIdUseCase useCase,
        [FromRoute] Guid id,
        [FromBody] RequestExpenseJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
}