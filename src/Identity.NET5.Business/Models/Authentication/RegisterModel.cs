using System.ComponentModel.DataAnnotations;

namespace Identity.NET5.Business.Models.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name é obrigatório")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password é obrigatório")]
        public string Password { get; set; }
    }
}
