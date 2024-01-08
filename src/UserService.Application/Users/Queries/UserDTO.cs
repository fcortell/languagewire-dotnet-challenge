using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Domain.Users.Entities;

namespace UserService.Application.Users.Queries
{
    public class UserDTO
    {
        public long Id { get; init; }

        public string? Name { get; init; }

        public string? Email { get; init; }

        public string Tier { get; set; } = string.Empty;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<User, UserDTO>();
            }
        }
    }
}
