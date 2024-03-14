using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public ClassesController(IClassService classService, IMapper mapper)
        {
            _classService = classService;
            _mapper = mapper;
        }

        [HttpPost("CreateClass")]
        public async Task<ActionResult> Index([FromBody] Pagination model)
        {
            var result = await _classService.LoadTable(model);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _classService.Create(_mapper.Map<Class>(model));
                return Ok(model);
            }
            return BadRequest();
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _classService.Update(_mapper.Map<Class>(model));
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
