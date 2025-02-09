using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Apis.DTOs
{
    public class LogInDto
    {
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress(ErrorMessage = "Invalid Email Format !!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required !!")]
        public string Password { get; set; }
    }
}
