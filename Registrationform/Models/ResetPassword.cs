using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace Registrationform.Models
{
    public class ResetPassword
    {
        public int ResetCode { get; set; }



        [Required(ErrorMessage = "Enter Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Enter valid password")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Confirm Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword")]
        [NotMapped]
        public string ConfirmPassword { get; set;}
    }
}
