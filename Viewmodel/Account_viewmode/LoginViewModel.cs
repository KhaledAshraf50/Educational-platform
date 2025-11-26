using System.ComponentModel.DataAnnotations;

namespace Luno_platform.Viewmodel.Account_viewmode
{
    public class LoginViewModel
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
