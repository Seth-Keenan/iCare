using System.ComponentModel.DataAnnotations;

namespace Group2_iCare.Models
{
    public class UserAuthentication
    {
        [Required(ErrorMessage = "Username field is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        public string Password { get; set; }

    }
}