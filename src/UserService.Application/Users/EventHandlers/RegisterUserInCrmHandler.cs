using System.Net.Http.Json;
using MediatR;
using UserService.Domain.Users.Entities;
using UserService.Domain.Users.Events;

namespace UserService.Application.Users.EventHandlers
{
    public class RegisterUserInCrmHandler : INotificationHandler<UserCreatedEvent>
    {
        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            RegisterInCrm(notification.User);
            return Task.CompletedTask;
        }

        private void RegisterInCrm(User user)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            var message = new HttpRequestMessage(HttpMethod.Post, "users");
            message.Content = JsonContent.Create(new { user.Name, user.Email });
            httpClient.Send(message);
        }
    }
}