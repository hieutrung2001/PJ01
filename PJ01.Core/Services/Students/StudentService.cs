﻿using AutoMapper;
using PJ01.Core.Helpers.Sorting;
using PJ01.Core.Interfaces.Repositories;
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Students;
using PJ01.Domain.Entities;

namespace PJ01.Core.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<Student>> GetAll()
        {
            var results = await _repository.QueryAsync();   
            return results.Select(x => new Student
            {
                Id = x.Id,
                FullName = x.FullName,
                Dob = x.Dob,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                StudentClasses = x.StudentClasses,
            }).ToList();
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
                x => x.FullName.Contains(model.Search.Value) 
                || x.Address.Contains(model.Search.Value)
                || x.PhoneNumber.ToString().Contains(model.Search.Value)
                || x.StudentClasses.Select(m => new Class
                {
                    Name = m.Class.Name
                }).Any(c => c.Name.Contains(model.Search.Value))
                );
            }

            var columnsName = new List<string>() { "FullName", "StudentClasses", "Address" };
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
                FullName = x.FullName,
                Dob = x.Dob,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                Classes = x.StudentClasses.Select(m => new Class
                {
                    Name = m.Class.Name,
                    Id = m.Class.Id
                }).ToList(),
            },
            !string.IsNullOrEmpty(model.Search.Value) ? x => x.FullName.Contains(model.Search.Value) 
            || x.Address.Contains(model.Search.Value)
            || x.PhoneNumber.ToString().Contains(model.Search.Value)
            || x.StudentClasses.Select(m => new Class
            {
                Name = m.Class.Name
            }).Any(c => c.Name.Contains(model.Search.Value))
            : null,
            m => SortingHelper.ApplyOrderBy(m, sortByInfo),
            pageSize: model.Length, page: model.Start / model.Length);
            
            return new JsonData<IndexModel> { Draw = model.Draw, RecordsFiltered = recordsFiltered, RecordsTotal = recordsTotal, Data = (List<IndexModel>)results };
        }

        public async Task<Student> GetStudentById(int id)
        {
            var result = await _repository.Get(id);
            return result;
        }

        public async Task<Student> Create(CreateViewModel model)
        {
            try
            {
                var student = _mapper.Map<Student>(model);
                await _repository.Add(student);
                return student;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Student> Update(EditViewModel student)
        {

            var result = await _repository.Get(student.Id);
            result.FullName = student.FullName;
            result.Address = student.Address;
            result.PhoneNumber = student.PhoneNumber;
            await _repository.Update(result);
            return result;
        }

        public async Task Delete(int id)
        {
            var student = await _repository.Get(id);
            if (student == null)
            {
                throw new Exception("Student not found!");
            }
            await _repository.Delete(student);
            
        }

    }
}
