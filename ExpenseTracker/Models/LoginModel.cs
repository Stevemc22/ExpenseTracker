using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class LoginModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}