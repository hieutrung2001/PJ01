using AutoMapper;
using PJ01.Core.ViewModels.Requests.Classes;
using PJ01.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
