﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBestTodoApp2020.Models;
using Library.Models;
using Library;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TheBestTodoApp2020.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController


        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserViewModel m)
        {
            DB db = new DB();

            if (!db.UserNameExists(m.UserName))
            {
                User user = new User();

                user.Email = m.Email;
                user.FirstName = m.FirstName;
                user.Password = m.Password;
                user.UserName = m.UserName;

                // SHA-512 Hashing for password
                //

                db.CreateUser(user);

                return View("UserCreated");
            }

            else
            {
                ViewData["takenUserName"] = m.UserName;
                return View("UserNameTaken");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel m) // edited
        {
            DB db = new DB();

            User user = db.GetUserByUserName(m.UserName);

            if (user != null && m.Password == user.Password)
            {
                db.LoginUser(user);

                ViewData["LoggedInUserName"] = m.UserName;

                //adding..

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, m.UserName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                //

                //return RedirectToAction("Index", "Todo", new { userName = user.UserName });
                return RedirectToAction("Index", "Todo");
            }

            else
            {
                return View("UserNotFound");
            }
        }

        public async Task<IActionResult> Logout()
        {
            DB db = new DB();

            var userName = User.FindFirstValue(ClaimTypes.Name);

            bool anyLoggedInUser = db.LogoutUser(userName);

            if (anyLoggedInUser)
            {
                await HttpContext.SignOutAsync(); // added

                return View("LoggedOut");
            }

            else
            {
                return View("NoUserToLogOut");
            }
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
