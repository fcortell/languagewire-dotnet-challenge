using Microsoft.AspNetCore.Mvc;
using UserService.Application.Common.Errors;
using UserService.Application.TranslationBalance.Commands;
using UserService.Application.Users.Queries;

namespace UserService.API.Controllers;

[ApiController]
[Route("translationBalance")]
public class TranslationBalanceController : BaseController
{
    /// <summary>
    /// Adds translation balance to user
    /// </summary>
    /// <response code="200">UserDTO</response>
    /// <response code="400">Validation error</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPost]
    [Route(nameof(AddTranslationBalance))]
    public async Task<IActionResult> AddTranslationBalance(
        [FromBody] AddTranslationBalanceCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        var firstError = result.Errors.FirstOrDefault();
        if (firstError is EntityNotFoundError)
        {
            return Problem(detail: firstError.Message, statusCode: StatusCodes.Status404NotFound);
        }
        return Problem();
    }

    /// <summary>
    /// Spend translation balance to user
    /// </summary>
    /// <response code="200">UserDTO</response>
    /// <response code="400">Validation error</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPost]
    [Route(nameof(SpendTranslationBalance))]
    public async Task<IActionResult> SpendTranslationBalance(
        [FromBody] SpendTranslationBalanceCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        var firstError = result.Errors.FirstOrDefault();
        if (firstError is InsufficientTranslationBalanceError)
        {
            return Problem(detail: firstError.Message, statusCode: StatusCodes.Status409Conflict);
        }
        if (firstError is EntityNotFoundError)
        {
            return Problem(detail: firstError.Message, statusCode: StatusCodes.Status404NotFound);
        }
        return Problem();
    }

    /// <summary>
    /// Susbstracts translation balance from user
    /// </summary>
    /// <response code="200">UserDTO</response>
    /// <response code="400">Validation error</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPost]
    [Route(nameof(SubstractTranslationBalance))]
    public async Task<IActionResult> SubstractTranslationBalance(
        [FromBody] SubstractTranslationBalanceCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        var firstError = result.Errors.FirstOrDefault();
        if (firstError is EntityNotFoundError)
        {
            return Problem(detail: firstError.Message, statusCode: StatusCodes.Status404NotFound);
        }
        return Problem();
    }
}