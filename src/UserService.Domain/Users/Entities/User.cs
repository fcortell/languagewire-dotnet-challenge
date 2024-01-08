using UserService.Domain.Common;

namespace UserService.Domain.Users.Entities
{
    public class User : BaseEntity
    {
        public string? Email { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public int TotalSpentAmount { get; set; } = 0;
        public int TranslationBalance { get; set; } = 0;
    }
}