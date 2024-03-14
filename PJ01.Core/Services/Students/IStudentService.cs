
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Students;
using PJ01.Domain.Entities;

namespace PJ01.Core.Services.Students
{
    public interface IStudentService
    {
        Task<JsonData<IndexModel>> LoadTable(Pagination model);
        Task<List<Student>> GetAll();
        Task<List<Student>> GetStudentsOfClass(int classId);
        Task<Student> GetStudentById(int id);
        Task<Student> Create(CreateViewModel student);
        Task<Student> Update(EditViewModel student);
        Task Delete(int id);
    }
}
