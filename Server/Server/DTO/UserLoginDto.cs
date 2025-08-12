using System.ComponentModel.DataAnnotations;

namespace Server.DTO
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Login ID is required.")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "OTP is required.")]
        [Range(100000, 999999, ErrorMessage = "OTP must be a 6-digit number.")]
        public int otp { get; set; }

    }
}