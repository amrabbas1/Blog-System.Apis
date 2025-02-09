using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Apis.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress(ErrorMessage = "Invalid Email Format !!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is Required !!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required !!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is Required !!")]
        [Compare(nameof(Password), ErrorMessage = "Confirmed password does not match password")]
        public string ConfirmPassword { get; set; }
    }
}
