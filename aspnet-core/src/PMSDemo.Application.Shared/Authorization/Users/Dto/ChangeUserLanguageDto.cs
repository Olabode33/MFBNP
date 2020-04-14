using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
