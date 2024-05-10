using System.ComponentModel.DataAnnotations;

namespace Talabat.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string DisplayName { get; set; }


        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$" ,ErrorMessage ="Minimum eight characters, at least one letter, one number and one special character:") ]
        public string Password { get; set; }


    }
}
