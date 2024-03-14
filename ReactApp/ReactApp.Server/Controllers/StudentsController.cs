using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PJ01.Core.Services.Students;
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Students;
using PJ01.Domain.Entities;

namespace PJ01.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        public StudentsController(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<ActionResult> Index([FromBody] Pagination model)
        {
            var result = await _studentService.LoadTable(model);
            return Ok(result);
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<EditViewModel>(student);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _studentService.Delete(id);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Student student = await _studentService.Create(model);
                    return Ok(student);
                } catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] EditViewModel model)
        {
            if (ModelState.IsValid && model.Id == id)
            {
                var result = await _studentService.Update(model);
                return Ok(result);
            }
            return BadRequest();
        }

    }
}
