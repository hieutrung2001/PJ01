using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PJ01.Core.Services.Students;
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Classes;
using PJ01.Domain.Entities;
using T1PJ.Core.Services.Classes;

namespace PJ01.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        public ClassesController(
            IClassService classService, 
            IMapper mapper,
            IStudentService studentService
            )
        {
            _classService = classService;
            _mapper = mapper;
            _studentService = studentService;
        }

        [HttpPost("")]
        public async Task<ActionResult> Index([FromBody] Pagination model)
        {
            var result = await _classService.LoadTable(model);
            //foreach (var item in result.Data)
            //{
            //    foreach (var item1 in item.StudentClasses)
            //    {
            //        var c = await _studentService.GetStudentById(item1.StudentId);
            //        item1.Student = new Student
            //        {
            //            FullName = c.FullName,
            //            Id = c.Id
            //        };
            //    }
            //}
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] EditModelAPI model)
        {
            if (ModelState.IsValid)
            {
                var results = new List<StudentClass>();
                if (model.StudentSelectList.Count > 0)
                {
                    foreach (var item in model.StudentSelectList)
                    {
                        results.Add(new StudentClass
                        {
                            StudentId = item,
                        });
                    }
                }
                var m = new CreateViewModel { Name = model.Name, StudentClasses = results };
                var c = _mapper.Map<Class>(m);
                await _classService.Create(c);
                return Ok(model);
            }
            return BadRequest();
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] EditModelAPI model)
        {
            if (ModelState.IsValid)
            {
                var results = new List<StudentClass>();
                foreach (var item in model.StudentSelectList)
                {
                    results.Add(new StudentClass
                    {
                        StudentId = item,
                        ClassId = id,
                    });
                }
                await _classService.Update(_mapper.Map<Class>(new EditViewModel
                {
                    Id = id,
                    Name = model.Name,
                    StudentClasses = results
                }));
                return Ok(model);
            }
            return BadRequest();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _classService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
