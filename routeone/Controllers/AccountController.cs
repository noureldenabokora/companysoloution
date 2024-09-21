using Demo.DAL.Models;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using routeone.Helpers;
using routeone.ViewModels;

namespace routeone.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppliactionUser> _userManager;
		private readonly SignInManager<AppliactionUser> _signInManager;
        private readonly IEmailSettings _emailSettings;
		private readonly ISmsServices _smsServices;

		public AccountController(UserManager<AppliactionUser> userManager, SignInManager<AppliactionUser> signInManager,IEmailSettings emailSettings,ISmsServices smsServices)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _emailSettings = emailSettings;
			_smsServices = smsServices;
		}

		#region Registier

		// /Account/Register
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid) //server side validation
			{
				var user = new AppliactionUser
				{
					Fname = model.Fname,
					Lname = model.Lname,
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					PhoneNumber = model.PhoneNumber,
				};

				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
					return RedirectToAction(nameof(Login));

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(model);
		}
		#endregion

		#region Login
		public IActionResult Login()
		{

			return View();

		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid) 
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{ 
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);
					if(flag)
					{
					  await	_signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						return RedirectToAction("Index", "Home");
					}
					ModelState.AddModelError(string.Empty, "Invalid Password");

				}
				ModelState.AddModelError(string.Empty, "Email is not Existd");
			}

			return View(model);

		}
		#endregion


		#region SignOut
		public new async Task<IActionResult> SignOut()
		{
		   await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion

		#region ForgetPassword
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model) 
		{
			if (ModelState.IsValid) 
			{
				var user =await _userManager.FindByEmailAsync(model.Email);
				if (user is not null) 
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var passwordRestLink = Url.Action("ResetPassword", "Account" , new {email = user.Email, token = token},Request.Scheme);
					var email = new Email()
					{
						Subject = "Rest Password",
						To = user.Email,
						Body = passwordRestLink
					};
					_emailSettings.SendEmail(email);
				// 	EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "Email is not Existed");
			}
			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> SendSms(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var passwordRestLink = Url.Action("ResetPassword", "Account", new { email = user.Email, token = token }, Request.Scheme);
					var sms = new SmsMessage()
					{
						PhoneNumber = user.PhoneNumber,
						Body = passwordRestLink,
					};
					_smsServices.Send(sms);
					return Ok("Check Your phone");
				}
				ModelState.AddModelError(string.Empty, "Email is not Existed");
			}
			return View(model);
		}


		public IActionResult CheckYourInbox()
		{
			return View();
		}
		#endregion

		#region Reset Password
		public IActionResult ResetPassword(string email , string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);

				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
				if (result.Succeeded)
					return RedirectToAction(nameof(Login));
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(model);
		}
		#endregion


		#region Google Login

		public IActionResult GoogleLogin() 
		{
			var prop = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
			 return  Challenge(prop,GoogleDefaults.AuthenticationScheme);
		}

		public async Task<IActionResult> GoogleResponse()
		{
		 var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
			var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
			{
				claim.Issuer,
				claim.OriginalIssuer,
				claim.Type,
				claim.Value,
			});

			return RedirectToAction("Index","Home");
		}
		#endregion
	}
}
