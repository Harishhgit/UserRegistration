using System.ComponentModel.DataAnnotations;


namespace Registrationform.Models
{
    public class ForgotPassword
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        
        public string Email { get; set; }

        public int ResetCode { get; set; }

    }
}
