using FluentResults;

namespace UserService.Application.Common.Errors;

public class UniqueConstraintViolationError : Error
{
    public UniqueConstraintViolationError(string entity, string property)
    {
        Message = $"Unique constraint violation on {entity}.{property}";
    }

    /// <summary>
    /// Property which must be unique
    /// </summary>
    public string Entity { get; set; } = null!;

    /// <summary>
    /// Property which must be unique
    /// </summary>
    public string Property { get; set; } = null!;
}