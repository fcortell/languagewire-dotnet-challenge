using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;

namespace UserService.Application.Common.Errors
{
    public class EntityNotFoundError : Error
    {
        /// <summary>
        /// Property which must be unique
        /// </summary>
        public long Id { get; set; }

        public EntityNotFoundError(long Id)
        {
            Message = $"Entity with Id: {Id} not found";
        }
    }
}
