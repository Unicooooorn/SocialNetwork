using System.ComponentModel;

namespace SocialNetwork.Api.Dto.Account
{
    public enum RolesDto
    {
        [Description("Админ")]
        Admin = 1,
        [Description("Модератор")]
        Moderator = 2,
        [Description("Пользователь")]
        User = 3
    }
}
