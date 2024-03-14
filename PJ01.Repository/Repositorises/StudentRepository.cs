using PJ01.Core.Interfaces.Repositories;
using PJ01.Domain.Entities;
using PJ01.Infrastructure.Context;
using PJ01.Infrastructure.Repositories;

namespace PJ01.Infrastructure.Repositorises
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(PJ01Context context) : base(context)
        {
        }
    }
}
