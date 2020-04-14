using System.ComponentModel.DataAnnotations;

namespace PMSDemo.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}