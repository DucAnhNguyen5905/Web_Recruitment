using System.ComponentModel.DataAnnotations;

namespace RecruitmentWebFE.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is null !")]
        [EmailAddress(ErrorMessage = "Invalid email format !")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Password is null !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

