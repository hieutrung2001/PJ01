using AutoMapper;
using PJ01.Domain.Entities;
using PJ01.Core.ViewModels.Requests.Students;

namespace PJ01.Core.Helpers.Mappers
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, IndexModel>();
            CreateMap<CreateViewModel, Student>();
            CreateMap<EditViewModel, Student>();
            CreateMap<Student, EditViewModel>();
        }
    }
}
