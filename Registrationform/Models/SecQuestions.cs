using System.ComponentModel.DataAnnotations;

namespace Registrationform.Models
{
    public class SecQuestions
    {
        [Key]
        public int SquestionId { get; set; }

        public string Squestion { get; set; }
    }
}
