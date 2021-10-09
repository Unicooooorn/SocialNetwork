using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Profiles
{
    public class Profile : IProfile
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано имя!")]
        [RegularExpression(@"[a-zA-Z]", ErrorMessage = "Неверный формат данных")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не указана фамилия!")]
        [RegularExpression(@"[a-zA-Z]", ErrorMessage = "Неверный формат данных")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Не указан возраст!")]
        public int Age { get; set; }

    }
}
