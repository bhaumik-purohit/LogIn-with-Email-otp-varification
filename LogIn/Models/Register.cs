using Microsoft.CodeAnalysis.Completion;
using System.ComponentModel.DataAnnotations;

namespace LogIn.Models
{
    public class Register
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "u_name")]
        [StringLength(30, ErrorMessage ="u_name not be exceed")]
        public string u_name { get; set; }


        [Required]
        [Display(Name = "user_name")]
        [StringLength(30, ErrorMessage = "user_name not be exceed")]
        public string user_name { get; set;}

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        public string pass_word { get; set;}

        [Required]
        [DataType(DataType.Password)]
        [Compare("pass_word")]
        public string  confirm_password { get; set;}

        [Required]
        [RegularExpression(".+\\@.+\\..+",ErrorMessage = "please enter valid email")] 
        public string email { get; set;}    


        [Required]
        [StringLength(10, ErrorMessage ="mobile_number not be exceed") ]
        public string mobile_number { get; set;}

        [Required(ErrorMessage = "enter date_of_birth")]
        public DateTime date_of_birth { get; set;}


        public string gender { get; set;}

        public string[] friends { get; set;}
    }
}
