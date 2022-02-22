using System.ComponentModel.DataAnnotations;

namespace Identity.NET5.Business.Models.Authentication
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "User Name é obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "New Password é obrigatório")]
        public string NewPassword { get; set; }
        
    }
}
