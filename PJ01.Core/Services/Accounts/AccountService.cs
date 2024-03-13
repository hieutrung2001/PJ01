using Microsoft.AspNetCore.Identity;
using PJ01.Core.ViewModels.Requests.Accounts;
using PJ01.Domain.Entities.Identity;

namespace PJ01.Core.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Register(RegisterViewModel model)
        {
            var user = new User { UserName = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
