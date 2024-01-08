using FluentResults;

namespace UserService.Application.Common.Errors
{
    public class EntityNotFoundError : Error
    {
        public EntityNotFoundError(long Id)
        {
            Message = $"Entity with Id: {Id} not found";
        }

        /// <summary>
        /// Property which must be unique
        /// </summary>
        public long Id { get; set; }
    }
}