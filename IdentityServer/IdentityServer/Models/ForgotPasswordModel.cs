using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
