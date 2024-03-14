using PJ01.Core.Interfaces.Repositories;
using PJ01.Domain.Entities;
using PJ01.Infrastructure.Context;
using PJ01.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ01.Infrastructure.Repositorises
{
    public class ClassRepository : RepositoryBase<Class>, IClassRepository
    {
        public ClassRepository(PJ01Context context) : base(context)
        {
        }
    }
}
