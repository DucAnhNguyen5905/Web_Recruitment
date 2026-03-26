using System.ComponentModel.DataAnnotations;

namespace RecruitmentWebFE.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email cannot be null or empty")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        [DataType(DataType.Password)]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
            ErrorMessage = "Password phải ≥8 ký tự, có chữ hoa, chữ thường, số và ký tự đặc biệt"
        )]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string ConfirmPassword { get; set; }
    }

}