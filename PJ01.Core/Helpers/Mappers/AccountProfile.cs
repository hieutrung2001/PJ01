using AutoMapper;
using PJ01.Core.ViewModels.Requests.Accounts;
using PJ01.Domain.Entities.Identity;

namespace PJ01.Core.Helpers.Mappers
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<LoginViewModel, User>();
            CreateMap<RegisterViewModel, User>();
        }
    }
}
