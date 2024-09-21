using AutoMapper;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.EntityFrameworkCore;
using routeone.ViewModels;

namespace routeone.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        #region index 
        // /Role/Index
        public async Task<IActionResult> Index(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                var Roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();  
                return View(Roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(Name);

                if (role is not null)
                {
                    var mappedRole = new RoleViewModel()
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    };

                    return View(new List<RoleViewModel>() { mappedRole });

                }
                return View(Enumerable.Empty<RoleViewModel>());

            }
        }

        #endregion

        // /Role/Create/Guid
        #region Create Role
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if(ModelState.IsValid)
            {
                var maapedRole = _mapper.Map<RoleViewModel,IdentityRole>(roleVM);
               await _roleManager.CreateAsync(maapedRole);
                return RedirectToAction(nameof(Index));
            }    
              
            return View(roleVM);
        }
        #endregion
       
        // /Role/Details/Guid
        #region User Details 
        public async Task<IActionResult> Details(string Id, string viewname = "Details")
        {
            if (Id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(Id);

            if (role is null)
                return NotFound();

            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);

            return View(viewname, mappedRole);
        }

        #endregion

        #region User Edit
        //Role/Edit/Guid
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            return await Details(Id, "Edit");
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel updatedRole)
        {
            if (id != updatedRole.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = updatedRole.RoleName;
                    


                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(updatedRole);
        }

        #endregion

        #region User Delete
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _roleManager.FindByIdAsync(Id);

            await _roleManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
