using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registrationform.Models
{
    public partial class Userdetails
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Enter Firstname")]
        [RegularExpression("^([a-zA-Z]{3,})", ErrorMessage = "Enter valid firstname")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Lastname")]
        [RegularExpression("^([a-zA-Z]{3,})", ErrorMessage = "Enter valid lastname")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set;}

        [MinimumAge(5,ErrorMessage ="Minimum age is 5 years")]
        [Required(ErrorMessage = "Enter the DateOfBirth")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Select the Country")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Enter Contact number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",ErrorMessage = "Entered phone format is not valid.")]
        [Display(Name = "Contact")]
        public long Contact { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Enter valid password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Pwd { get; set;}

        [Required(ErrorMessage = "Confirm Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Pwd",ErrorMessage ="Password does not match!")]
        [NotMapped]
        public string Confirmpwd { get; set; }

        [Required(ErrorMessage = "Select the Gender")]
        [DataType(DataType.Text)]
        [Display(Name = "Gender")]
        public string Gender { get; set;}


        [Display(Name = "SecurityQuestion")]
        public int QuestionId { get; set; }


        [Display(Name = "Answer")]
        public string Answers { get; set;}

        [Display(Name = "State")]
        public string State { get; set;}

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name="Profile Photo")]
        [NotMapped]
        public IFormFile? ProfilePhoto { get; set; }

        public string? ProfilePhotoUrl { get; set; }

    }

}
