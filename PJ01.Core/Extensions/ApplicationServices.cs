using Microsoft.Extensions.DependencyInjection;
using PJ01.Core.Services.Accounts;
using PJ01.Core.Services.Students;
using PJ01.Core.Services.Token;
using T1PJ.Core.Services.Classes;

namespace PJ01.Core.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
