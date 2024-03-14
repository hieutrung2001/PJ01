using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PJ01.Core.Services.Students;
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Students;
using PJ01.Domain.Entities;

namespace PJ01.AppMVC.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStudentService _service;

        public StudentsController(IMapper mapper, IStudentService service)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<IActionResult> Index(int? id)
        {
            //var students = await _service.GetAll();
            //if (id != null)
            //{
            //    var model1 = _mapper.Map<List<IndexModel>>(await _service.GetStudentsOfClass((int)id));
            //    return View(model1);
            //}
            //var model = _mapper.Map<List<IndexModel>>(students);
            return View();
        }

        public IActionResult Create()
        {
            CreateViewModel model = new CreateViewModel
            {
                Dob = DateTime.Now,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _service.Create(model) != null)
                {
                    return Json(new { status = true });
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await _service.GetStudentById(id) == null)
            {
                return Json(new { status = false });
            }
            var result = await _service.GetStudentById(id);
            EditViewModel model = _mapper.Map<EditViewModel>(result);
            return View(model);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _service.Update(model) != null)
                {
                    return Json(new { status = true });
                }
            }
            return View(model);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);
                return Json(new { status = true });

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            if (await _service.GetStudentById(id) == null)
            {
                return Json(new { status = false });
            }
            var result = await _service.GetStudentById(id);

            return View(_mapper.Map<EditViewModel>(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LoadTable(Pagination model)
        {
            JsonData<IndexModel> result = await _service.LoadTable(model);
            var jsonData = new { draw = result.Draw, recordsFiltered = result.RecordsFiltered, recordsTotal = result.RecordsTotal, data = result.Data };
            return Json(jsonData);

        }
    }
}
