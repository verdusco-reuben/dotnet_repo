using System.ComponentModel.DataAnnotations;

namespace beltexam.Models
{
    public class UserViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        [RegularExpression("[^0-9]", ErrorMessage = "Name can not contain numbers.")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression("[^0-9]", ErrorMessage = "Name can not contain numbers.")]
        public string LastName { get; set; }
 
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
 
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [RegularExpression("[?0-9][A-Za-z]+", ErrorMessage = "Password must contain 1 number, 1 letter, and 1 special char.")]
        public string Password { get; set; }
 
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        public string PasswordConfirmation { get; set; }
    }
}