using AutoMapper;
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
                var filtered = await _repository.QueryAsync(
                    !string.IsNullOrEmpty(model.Search.Value) ? x => x.FullName.Contains(model.Search.Value) || x.Address.Contains(model.Search.Value) : null);
                recordsFiltered = filtered.Count();
            }
            var results = await _repository.QueryAndSelectAsync(selector: x => new IndexModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Dob = x.Dob,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                StudentClasses = x.StudentClasses,
            },
            !string.IsNullOrEmpty(model.Search.Value) ? x => x.FullName.Contains(model.Search.Value) || x.Address.Contains(model.Search.Value) : null,
            pageSize: model.Length, page: model.Start / model.Length);
            //if (model.Order != null)
            //{
            //    if (model.Order[0].Dir == "asc")
            //    {
            //        if (model.Order[0].Column == 0)
            //        {
            //            results = results.OrderBy(data => data.FullName).ToList();
            //        } else if (model.Order[0].Column == 2)
            //        {
            //            results = results.OrderBy(data => data.Address).ToList();
            //        }
            //    }
            //    else
            //    {
            //        if (model.Order[0].Column == 0)
            //        {
            //            results = results.OrderByDescending(data => data.FullName).ToList();
            //        }
            //        else if (model.Order[0].Column == 2)
            //        {
            //            results = results.OrderByDescending(data => data.Address).ToList();
            //        }
            //    }
            //}
            
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

        public async Task<List<Student>> GetStudentsOfClass(int classId)
        {
            var studentsOfClass = new List<Student>();
            var students = await GetAll();
            if (students != null)
            {
                foreach (var item in students)
                {
                    if (item.StudentClasses?.Count > 0)
                    {
                        foreach (var y in item.StudentClasses)
                        {
                            if (y.ClassId == classId)
                            {
                                studentsOfClass.Add(item);
                                break;
                            }
                        }
                    }
                }
                return studentsOfClass;
            }
            return [];
        }

    }
}
