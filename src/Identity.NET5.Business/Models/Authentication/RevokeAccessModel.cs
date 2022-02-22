using System.ComponentModel.DataAnnotations;

namespace Identity.NET5.Business.Models.Authentication
{
    public class RevokeAccessModel
    {
        [Required(ErrorMessage = "User Name é obrigatório")]
        public string Username { get; set; }
    }
}
