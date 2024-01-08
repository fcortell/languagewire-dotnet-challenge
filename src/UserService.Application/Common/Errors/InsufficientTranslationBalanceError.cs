using FluentResults;

namespace UserService.Application.Common.Errors
{
    public class InsufficientTranslationBalanceError : Error
    {
        public InsufficientTranslationBalanceError(long Id)
        {
            Message = $"Entity with Id: {Id} has insufficient Translation balance. Can't spend amount requested.";
        }
    }
}