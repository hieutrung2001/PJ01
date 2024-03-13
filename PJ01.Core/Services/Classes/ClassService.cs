using Microsoft.EntityFrameworkCore;
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Classes;
using PJ01.Domain.Context;
using PJ01.Domain.Entities;

namespace T1PJ.Core.Services.Classes
{
    public class ClassService : IClassService
    {
        private readonly PJ01Context _context;

        public ClassService(PJ01Context context)
        {
            _context = context;
        }

        public async Task<JsonData<IndexModel>> LoadTable(Pagination model)
        {
            int recordsTotal = await _context.Classes.CountAsync();
            int recordsFiltered = recordsTotal;
            var results = await _context.Classes.AsNoTracking().Select(x => new IndexModel
            {
                Id = x.Id,
                Name = x.Name,
                StudentClasses = x.StudentClasses,
            }).Skip(model.Start).Take(model.Length).ToListAsync();
            if (model.Order != null)
            {
                if (model.Order[0].Dir == "asc")
                {
                    if (model.Order[0].Column == 0)
                    {
                        results = results.OrderBy(data => data.Name).ToList();
                    }
                }
                else
                {
                    if (model.Order[0].Column == 0)
                    {
                        results = results.OrderByDescending(data => data.Name).ToList();
                    }
                }
            }
            if (!string.IsNullOrEmpty(model.Search.Value))
            {
                results = results.Where(m => m.Name.ToLower().Contains(model.Search.Value.ToLower())).ToList();
                recordsFiltered = results.Count();
            }

            return new JsonData<IndexModel> { Draw = model.Draw, RecordsFiltered = recordsFiltered, RecordsTotal = recordsTotal, Data = results };
        }
        public async Task<List<Class>> GetAll()
        {
            return await _context.Classes.AsNoTracking().Select(x => new Class
            {
                Id = x.Id,
                Name = x.Name,
                StudentClasses = x.StudentClasses,
            }).ToListAsync();
        }

        public async Task<Class> GetClassById(int id)
        {
            var result = await _context.Classes.AsNoTracking().Select(x => new Class
            {
                Id = x.Id,
                Name = x.Name,
                StudentClasses = x.StudentClasses,
            }).FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task Create(Class c)
        {
            _context.Classes.Add(c);
            await _context.SaveChangesAsync();
            if (c.StudentClasses?.Count > 0)
            {
                foreach (var item in c.StudentClasses)
                {
                    item.ClassId = c.Id;
                    _context.StudentClasses.Add(new StudentClass { ClassId = c.Id, StudentId = item.StudentId });
                }
                _context.Classes.Update(c);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Class c)
        {
            var c1 = _context.Classes.Select(x => new Class
            {
                Id = x.Id,
                StudentClasses = x.StudentClasses,
            }).FirstOrDefault(s => s.Id == c.Id);
            c1.Name = c.Name;
            if (c1.StudentClasses.Count > 0)
            {
                var results = c.StudentClasses;
                List<bool> checks = new List<bool>(results.Count);
                checks.AddRange(Enumerable.Repeat(false, results.Count));
                var j = 0;
                foreach (var item in c1.StudentClasses)
                {
                    if (c.StudentClasses.Contains(item))
                    {
                        checks[j++] = true;
                        continue;
                    }
                    else
                    {
                        _context.StudentClasses.Remove(item);
                        
                    }
                }
                for (var i = 0; i < c.StudentClasses.Count; ++i)
                {
                    if (!checks[i])
                    {
                        var studentClass = new StudentClass { ClassId = c.Id, StudentId = c.StudentClasses[i].StudentId };
                        c1.StudentClasses.Add(studentClass);
                        _context.StudentClasses.Add(studentClass);
                    }
                }
                //_context.Classes.Update(c1);
            } else
            {
                foreach (var item in c.StudentClasses)
                {
                    var studentClass = new StudentClass { ClassId = c.Id, StudentId = item.StudentId };
                    c1.StudentClasses.Add(studentClass);
                    _context.StudentClasses.Add(studentClass);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var c = _context.Classes.Find(id);
            if (c is null)
            {
                throw new Exception("Class not found!");
            }
            _context.Classes.Remove(c);
            await _context.SaveChangesAsync();
        }
    }
}
