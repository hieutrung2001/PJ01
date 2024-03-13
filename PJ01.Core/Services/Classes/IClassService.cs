
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Classes;
using PJ01.Domain.Entities;

namespace T1PJ.Core.Services.Classes
{
    public interface IClassService
    {
        Task<JsonData<IndexModel>> LoadTable(Pagination model);
        Task<List<Class>> GetAll();
        Task<Class> GetClassById(int id);
        Task Create(Class c);
        Task Update(Class c);
        Task Delete(int id);
    }
}
