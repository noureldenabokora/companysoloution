using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using routeone.ViewModels;

namespace routeone.Controllers
{
    public class DepartmentController : Controller
    {

        //  private readonly IDepartmentRepository _departmentRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper)
          //  IDepartmentRepository departmentRepository
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //   _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var dept = await _unitOfWork.DepartmentRepository.GetAll();
          await  _unitOfWork.Complete();
            return View(dept);
        }
        //Department/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) //server side validation
            {
                //////// i am here
                var mappeddapt = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                await _unitOfWork.DepartmentRepository.Add(mappeddapt);
              await  _unitOfWork.Complete();
         //       if(count >0)
         //         TempData["Message"] = "Department is Created Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }

        public async Task<IActionResult> Details(int? Id,string viewname ="Details")
        {
            if (Id is null)
                return BadRequest();

            var department =  await _unitOfWork.DepartmentRepository.Get(Id.Value);
            if(department is null)
                return NotFound();

            return View(viewname,department);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id) 
        {
            return await Details(Id,"Edit");
          /*  if(Id is null)
                return BadRequest();
            var department = _departmentRepository.Get(Id.Value);
            if (department is null)
                return NotFound();
            return View(department);*/
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {
            if(id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                  await  _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(department);
        }

        public async Task<IActionResult> Delete(int Id) 
        {
            var deleteddep = await _unitOfWork.DepartmentRepository.Get(Id);
            _unitOfWork.DepartmentRepository.Delete(deleteddep);
            await _unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }
    }
}
