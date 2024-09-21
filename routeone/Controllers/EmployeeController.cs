using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using routeone.Helpers;
using routeone.ViewModels;

namespace routeone.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        //private readonly IEmployeeRepository _employeeRepository;
        // private readonly IDepartmentRepository _departmentRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork , IMapper mapper)       
//IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository
           
        {
            //  _employeeRepository = employeeRepository;
            //  _departmentRepository = departmentRepository;

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Employee/Index
        public async Task<IActionResult> Index()
        {
            ViewData["Message"] = "Hello from viewData";

           // ViewBag.Message = "Hello from viewBag";
            var emp = await _unitOfWork.EmployeeRepository.GetAll();

            var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(emp);

            return View(mappedEmp);
        }
        //Employee/Create
        [HttpGet]
        public IActionResult Create()
        {
           // ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) //server side validation
            {
                //manual Mapping
             /*   var employee = new Employee()
                {
                    Name = employeeVM.Name,
                    Address = employeeVM.Address,
                    Email = employeeVM.Email,
                    Age = employeeVM.Age,
                    Phone = employeeVM.Phone,
                    IsAcitve = employeeVM.IsAcitve,
                    HireDate = employeeVM.HireDate,
                    DepartmentId = employeeVM.DepartmentId,
                    Salary = employeeVM.Salary,

                };*/
            
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "images");
                var mappedEmp = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                
                await _unitOfWork.EmployeeRepository.Add(mappedEmp);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? Id, string viewname = "Details")
        {
            if (Id is null)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.Get(Id.Value);
            if (employee is null)
                return NotFound();

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(viewname, mappedEmp);
        }
       
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            return await Details(Id, "Edit");
            /*  if(Id is null)
                  return BadRequest();
              var Employee = _EmployeeRepository.Get(Id.Value);
              if (Employee is null)
                  return NotFound();
              return View(Employee);*/
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedemp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedemp); 

                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(employeeVM);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var deleteddep = await _unitOfWork.EmployeeRepository.Get(Id);
            _unitOfWork.EmployeeRepository.Delete(deleteddep);
           int count= await _unitOfWork.Complete();
            if (count > 0)
                DocumentSettings.DeleteFile(deleteddep.ImageName, "images");

            return RedirectToAction(nameof(Index));
        }
    
}
}
