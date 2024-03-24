﻿using Microsoft.EntityFrameworkCore;
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Classes;
using PJ01.Domain.Entities;
using PJ01.Core.Interfaces.Repositories;
using PJ01.Core.Helpers.Sorting;

namespace T1PJ.Core.Services.Classes
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _repository;

        public ClassService(IClassRepository classRepository)
        {
            _repository = classRepository;
        }

        public async Task<JsonData<IndexModel>> LoadTable(Pagination model)
        {
            var records = await _repository.QueryAsync();
            int recordsTotal = records.Count;
            int recordsFiltered = recordsTotal;

            // recordsFiltered
            if (!string.IsNullOrEmpty(model.Search.Value))
            {
                recordsFiltered = await _repository.CountAsync(
                x => x.Name.Contains(model.Search.Value)
                || x.StudentClasses.Select(m => new Student
                {
                    FullName = m.Student.FullName
                }).Any(c => c.FullName.Contains(model.Search.Value.Trim()))
                );
            }

            // sorting
            var columnsName = new List<string>() { "Name", "StudentClasses" };
            var sortDirection = SortingHelper.SortDirection.Ascending;
            if (model.Order != null)
            {
                sortDirection = model.Order[0].Dir == "asc" ? SortingHelper.SortDirection.Ascending : SortingHelper.SortDirection.Descending;
            }
            var sortByInfo = new SortingHelper.SortByInfo
            {
                Direction = sortDirection,
                PropertyName = model.Order != null ? columnsName[model.Order[0].Column] : "",

            };

            var results = await _repository.QueryAndSelectAsync(x => new IndexModel
            {
                Id = x.Id,
                Name = x.Name,
                Students = x.StudentClasses.Select(m => new Student
                {
                    Id = m.Student.Id,
                    FullName = m.Student.FullName,
                    Dob = m.Student.Dob,
                    Address = m.Student.Address,
                }).ToList(),
            },
            !string.IsNullOrEmpty(model.Search.Value) ? x => x.Name.Contains(model.Search.Value)
            || x.StudentClasses.Select(m => new Student
            {
                FullName = m.Student.FullName
            }).Any(c => c.FullName.Contains(model.Search.Value.Trim()))
            : null,
            m => SortingHelper.ApplyOrderBy(m, sortByInfo),
            pageSize: model.Length, page: model.Start / model.Length);

            return new JsonData<IndexModel> { Draw = model.Draw, RecordsFiltered = recordsFiltered, RecordsTotal = recordsTotal, Data = (List<IndexModel>)results };
        }

        public async Task<Class> GetClassById(int id)
        {
            var result = await _repository.Get(id, "StudentClasses");
            if (result == null)
            {
                throw new Exception("Class not found!");
            }
            return result;
        }

        public async Task Create(Class c)
        {
            await _repository.Add(c);
            //if (c.StudentClasses?.Count > 0)
            //{
            //    foreach (var item in c.StudentClasses)
            //    {
            //        item.ClassId = c.Id;
            //        //_context.StudentClasses.Add(new StudentClass { ClassId = c.Id, StudentId = item.StudentId });
            //    }
            //    await _repository.Update(c);
            //}
        }

        public async Task Update(Class c)
        {
            var c1 = await _repository.Get(c.Id, "StudentClasses");
            c1.Name = c.Name;
            if (c1.StudentClasses?.Count > 0)
            {
                var results = c.StudentClasses;
                List<bool> checks = new List<bool>(100);
                checks.AddRange(Enumerable.Repeat(false, 100));
                var deleteElements = new List<StudentClass>();
                foreach (var item in c1.StudentClasses)
                {
                    if (c.StudentClasses.FirstOrDefault(x => x.StudentId == item.StudentId) != null)
                    {
                        checks[item.StudentId] = true;
                        continue;
                    }
                    else
                    {
                        deleteElements.Add(item);
                    }
                }
                if (deleteElements.Count > 0)
                {
                    foreach (var item in deleteElements)
                    {
                        c1.StudentClasses.Remove(item);                        
                    }
                }
                for (var i = 0; i < c.StudentClasses.Count; ++i)
                {
                    if (!checks[c.StudentClasses[i].StudentId])
                    {
                        var studentClass = new StudentClass { 
                            ClassId = c.Id, 
                            StudentId = c.StudentClasses[i].StudentId,
                        };
                        c1.StudentClasses.Add(studentClass);
                    }
                }
                //_context.Classes.Update(c1);
            } else
            {
                c1.StudentClasses = new List<StudentClass>();
                foreach (var item in c.StudentClasses)
                {
                    var studentClass = new StudentClass { 
                        ClassId = c.Id, 
                        StudentId = item.StudentId,
                    };
                    c1.StudentClasses.Add(studentClass);
                }
            }
            await _repository.Update(c1);
        }

        public async Task Delete(int id)
        {
            var c = await _repository.Get(id);
            if (c is null)
            {
                throw new Exception("Class not found!");
            }
            await _repository.Delete(c);
        }
    }
}
