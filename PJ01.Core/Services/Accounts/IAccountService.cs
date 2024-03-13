
using PJ01.Core.ViewModels.Requests.Accounts;

namespace PJ01.Core.Services.Accounts
{
    public interface IAccountService
    {
        Task<bool> Login(LoginViewModel model);
        Task<bool> Register(RegisterViewModel model);
        Task Logout();
    }
}
