using System.ComponentModel.DataAnnotations;

namespace PJ01.Core.ViewModels.Requests.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter your password"), DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
