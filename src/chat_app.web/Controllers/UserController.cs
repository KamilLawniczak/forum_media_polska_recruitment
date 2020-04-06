using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chat_app.domain;
using chat_app.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chat_app.web.Controllers
{
    public class UserController : Controller
    {
        private readonly ChatUserService _chatUserService;

        public UserController(ChatUserService chatUserService)
        {
            _chatUserService = chatUserService;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(UserLogInModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _chatUserService.TryLogIn (
                    loginModel.Name.Trim (),
                    loginModel.Password.Trim ());
                
                if (result.success)
                {
                    var claims = new List<Claim>
                        {
                            new Claim("UserId", result.user.Id.ToString()),
                            new Claim("UserName", result.user.Name)
                        };

                    var claimsIdentity = new ClaimsIdentity (
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes (20),
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync (
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal (claimsIdentity),
                            authProperties);

                    return RedirectToAction ("Index", "Chat");
                }
            }

            return View ();
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync ();
            return RedirectToAction ("Index", "Chat");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                await _chatUserService.AddNewUser (registerModel.Name.Trim (), registerModel.Password.Trim ());
            }

            return View ();
        }
    }
}