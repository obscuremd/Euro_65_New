using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string FullName { get; set; }
        
        [Required]
        public string Address { get; set; }
        
        [Required]
        public  string Email { get; set; }
        
        [Required]
        public  string PhoneNumber { get; set; }
        
        [Required]
        public  string Sex { get; set; }
        
        [Required]
        public  string Nin { get; set; }
       
        [Required]
        public  string Branch { get; set; }
        
        [Required]
        public  string Role { get; set; } //Admin , Secretary, Dealer, Boy

        [Required]
        public string ProfilePicture { get; set; }
        //Login fields

        public string? LoginId { get; set; }

        public bool? isAuthenticated { get; set; }

        public DateTime? AuthenticatedAt {get; set;}

        public int? otp { get; set; }

        public DateTime? OtpSentAt { get; set; }

        //relationships
        public ICollection<Car>? Cars { get; set; }

        public static implicit operator User(ValueTask<User?> v)
        {
            throw new NotImplementedException();
        }
    }
}