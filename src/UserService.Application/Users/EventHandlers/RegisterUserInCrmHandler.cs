using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using UserService.Application.Common.Errors;
using UserService.Domain.Users.Entities;
using UserService.Domain.Users.Events;
using UserService.Domain.Users;
using System.Net.Http.Json;

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
