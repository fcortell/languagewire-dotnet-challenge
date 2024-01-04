using FluentResults;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Users.Commands.CreateUser;
using UserService.Application.Users.Queries;
using UserService.Application.Common.Errors;
using UserService.Application.Users.Commands.UpdateUser;

namespace UserService.API.Controllers;

[ApiController]
[Route("users")]
public class UserController : BaseController
{
    /// <summary>
    /// Gets a user by ID
    /// </summary>
    /// <response code="200">The user</response>
    /// <response code="404">User not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetById(
       [FromRoute] long id, CancellationToken cancellationToken)
    {
        GetUserByIdQuery query = new GetUserByIdQuery
        {
            UserId = id
        };

        Result<UserDTO> result = await Mediator.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem();
    }

    /// <summary>
    /// Creates a user
    /// </summary>
    /// <response code="200">ID of the created user</response>
    /// <response code="400">Validation error</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        var firstError = result.Errors.FirstOrDefault();
        if (firstError is UniqueConstraintViolationError)
        {
            return Problem(detail: firstError.Message, statusCode: StatusCodes.Status409Conflict);
        }
        return Problem();
    }

    /// <summary>
    /// Updates a user
    /// </summary>
    /// <response code="200">UserDTO</response>
    /// <response code="400">Validation error</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserCommand command)
    {
        var result = await Mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        var firstError = result.Errors.FirstOrDefault();
        if (firstError is UniqueConstraintViolationError)
        {
            return Problem(detail: firstError.Message, statusCode: StatusCodes.Status409Conflict);
        }
        if (firstError is EntityNotFoundError)
        {
            return Problem(detail: firstError.Message, statusCode: StatusCodes.Status404NotFound);
        }
        return Problem();
    }
}