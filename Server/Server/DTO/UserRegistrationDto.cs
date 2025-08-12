using System.ComponentModel.DataAnnotations;

namespace Server.DTO
{
    public class UserRegistrationDto
    {
        [Required, MinLength(2, ErrorMessage = "Full name is too short.")]
        public string FullName { get; set; }

        [Required, MinLength(6, ErrorMessage = "Address is too short.")]
        public string Address { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required, Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required, MinLength(10, ErrorMessage = "NIN must be at least 10 characters long.")]
        public string Nin { get; set; }

        [Required, MinLength(2, ErrorMessage = "Branch name is too short.")]
        public string Branch { get; set; }

        public string? ProfilePicture { get; set; }

    }
}
