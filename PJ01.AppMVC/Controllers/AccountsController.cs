using Microsoft.AspNetCore.Mvc;
using PJ01.Core.Services.Accounts;
using PJ01.Core.ViewModels.Requests.Accounts;

namespace PJ01.AppMVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // login
        public IActionResult Login(string? returnUrl = "")
        {
            if (User.Identity != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Students");
                }
            }
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _accountService.Login(model))
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Students");
                    }
                }
            }
            return View(model);
        }

        // register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _accountService.Register(model))
                {
                    return RedirectToAction("Index", "Students");

                }
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Login");
        }
    }
}
