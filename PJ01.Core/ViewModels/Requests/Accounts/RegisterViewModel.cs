using System.ComponentModel.DataAnnotations;

namespace PJ01.Core.ViewModels.Requests.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter your password"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please re-enter your password"), DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
