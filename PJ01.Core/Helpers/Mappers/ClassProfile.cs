using AutoMapper;
using PJ01.Core.ViewModels.Requests.Classes;
using PJ01.Domain.Entities;

namespace PJ01.Core.Helpers.Mappers
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, IndexModel>();
            CreateMap<CreateViewModel, Class>();
            CreateMap<Class, EditViewModel>();
            CreateMap<EditViewModel, Class>();
        }
    }
}
