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