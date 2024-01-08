using UserService.Application.Common.Interfaces;

namespace UserService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}