using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Common;
using UserService.Domain.Users.Entities;

namespace UserService.Domain.Users.Events
{
    public class UserCreatedEvent : BaseEvent
    {
        public UserCreatedEvent(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
