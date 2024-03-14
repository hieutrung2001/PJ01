using System.ComponentModel.DataAnnotations;

namespace PJ01.Core.ViewModels.Requests.Students
{
    public class EditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
