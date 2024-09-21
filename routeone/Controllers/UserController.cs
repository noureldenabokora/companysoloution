using AutoMapper;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using routeone.Helpers;
using routeone.ViewModels;
using System.Diagnostics;

namespace routeone.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<AppliactionUser> _userManager;
		private readonly SignInManager<AppliactionUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppliactionUser> userManager, SignInManager<AppliactionUser> signInManager,IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _mapper = mapper;
        }

		#region index 
		// /User/Index
		public async Task<IActionResult> Index(string Email)
		{
			if (string.IsNullOrEmpty(Email))
			{
				var users = await _userManager.Users.Select(u => new UserViewModel()
				{
					Id = u.Id,
					Email = u.Email,
					FName = u.Fname,
					LName = u.Lname,
					PhoneNumber = u.PhoneNumber,
					Roles = _userManager.GetRolesAsync(u).Result,
				}).ToListAsync();
				return View(users);
			}
			else
			{
				var users = await _userManager.FindByEmailAsync(Email);
				var mappedUser = new UserViewModel()
				{
					Id = users.Id,
					Email = users.Email,
					FName = users.Fname,
					LName = users.Lname,
					PhoneNumber = users.PhoneNumber,
					Roles = _userManager.GetRolesAsync(users).Result,

				};

				return View(new List<UserViewModel>() { mappedUser });

			}
		}

        #endregion

        #region User Details 
        public async Task<IActionResult> Details(string Id, string viewname = "Details")
        {
            if (Id is null)
                return BadRequest();

            var user =  await _userManager.FindByIdAsync(Id);

            if (user is null)
                return NotFound();

            var mappedUser = _mapper.Map<AppliactionUser, UserViewModel>(user);

            return View(viewname, mappedUser);
        }

        #endregion

        #region User Edit
        //User/Edit/Guid
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            return await Details(Id, "Edit");
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var user =await _userManager.FindByIdAsync(id);
                    user.Fname = updatedUser.FName;
                    user.Lname =  updatedUser.LName;
                    user.PhoneNumber   = updatedUser.PhoneNumber; 
                    
                    

                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(updatedUser);
        }

        #endregion

        #region User Delete
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
        #endregion
     }
}
