using System.ComponentModel.DataAnnotations;

namespace Identity.NET5.Business.Models.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name é obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password é obrigatório")]
        public string Password { get; set; }
    }
}
