using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PJ01.Core.Services.Accounts;
using PJ01.Core.Services.Token;
using PJ01.Core.ViewModels.Requests.Accounts;
using PJ01.Core.ViewModels.Responses.Users;
using PJ01.Domain.Entities.Identity;
using System.Security.Claims;

namespace PJ01.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private ITokenService _tokenService;
        private IAccountService _accountService;
        
        public AccountsController(ITokenService tokenService, IAccountService accountService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
        }

        private async Task<bool> AuthenticateUser(LoginViewModel model)
        {
            return await _accountService.Login(model);

        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            IActionResult response = Unauthorized();
            if (await AuthenticateUser(model))
            {
                var token = _tokenService.GenerateAccessToken();
                response = Ok(new { token });
            }
            return response;
        }
    }
}
