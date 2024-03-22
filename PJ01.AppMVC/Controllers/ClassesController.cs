using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PJ01.Core.Services.Students;
using PJ01.Core.ViewModels.Paginations;
using PJ01.Core.ViewModels.Requests.Classes;
using PJ01.Domain.Entities;
using T1PJ.Core.Services.Classes;

namespace PJ01.AppMVC.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IClassService _classService;
        private readonly IStudentService _studentService;

        public ClassesController(IMapper mapper, IClassService classService, IStudentService studentService)
        {
            _mapper = mapper;
            _classService = classService;
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var students = await _studentService.GetAll();
            ViewBag.StudentSelectList = new SelectList(
                    items: students,
                    dataValueField: nameof(Student.Id),
                    dataTextField: nameof(Student.FullName)
                );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] string Name, [FromForm] List<string> StudentSelectList)
        {
            if (ModelState.IsValid)
            {
                var results = new List<StudentClass>();
                if (StudentSelectList.Count > 0)
                {
                    foreach (var item in StudentSelectList)
                    {
                        results.Add(new StudentClass { 
                            StudentId = Int32.Parse(item), 
                        });
                    }
                }
                var model = new CreateViewModel { Name = Name, StudentClasses = results };
                var c = _mapper.Map<Class>(model);
                await _classService.Create(c);
                return Json(new { status = true });
            }
            return View();
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _classService.Delete(id);
                return Json(new { status = true });

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await _classService.GetClassById(id) == null)
            {
                return Json(new { status = false });
            }
            var result = await _classService.GetClassById(id);
            var ids = new List<int>();
            if (result.StudentClasses?.Count > 0)
            {
                foreach (var item in result.StudentClasses)
                {
                    ids.Add(item.StudentId);
                }
            }
            ViewBag.StudentSelectList = new SelectList(
                    items: await _studentService.GetAll(),
                    dataValueField: nameof(Student.Id),
                    dataTextField: nameof(Student.FullName)
                );

            var model = _mapper.Map<EditViewModel>(result);
            model.StudentSelectList = ids;
            return View(model);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] int Id, [FromForm] string Name, [FromForm] List<string> StudentSelectList)
        {
            if (ModelState.IsValid)
            {
                var results = new List<StudentClass>();
                foreach (var item in StudentSelectList)
                {
                    results.Add(new StudentClass { 
                        StudentId = Int32.Parse(item), 
                        ClassId = Id,
                    });
                }
                await _classService.Update(_mapper.Map<Class>(new EditViewModel
                {
                    Id = Id,
                    Name = Name,
                    StudentClasses = results
                }));
                return Json(new { status = true });
            }
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var c = await _classService.GetClassById(id);
            if (c.StudentClasses.Any())
            {
                foreach (var item in c.StudentClasses)
                {
                    var s = await _studentService.GetStudentById(item.StudentId);
                    item.Student = new Student { FullName = s.FullName };
                }
            }
            return View(_mapper.Map<EditViewModel>(c));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LoadTable(Pagination model)
        {
            JsonData<IndexModel> result = await _classService.LoadTable(model);
            foreach (var item in result.Data)
            {
                foreach (var item1 in item.StudentClasses)
                {
                    var c = await _studentService.GetStudentById(item1.StudentId);
                    item1.Student = new Student
                    {
                        FullName = c.FullName,
                        Id = c.Id
                    };
                }
            }
            var jsonData = new { 
                draw = result.Draw, 
                recordsFiltered = result.RecordsFiltered, 
                recordsTotal = result.RecordsTotal, 
                data = result.Data 
            };
            return Json(jsonData);

        }
    }
}
