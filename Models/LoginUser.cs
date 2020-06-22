using System.ComponentModel.DataAnnotations;

namespace CsharpBelt.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [MinLength(8)]
        [DataType("Password")]
        public string Password {get;set;}
    }
}