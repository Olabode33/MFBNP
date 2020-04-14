using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}