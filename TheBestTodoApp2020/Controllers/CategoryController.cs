using Library;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using TheBestTodoApp2020.Models;

namespace TheBestTodoApp2020.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // GET: CategoryController
        public ActionResult Index()
        {
            DB db = new DB();
            var userName = User.FindFirstValue(ClaimTypes.Name);
            User user = db.GetUserByUserName(userName);
            ViewData["userName"] = userName;

            List<Category> allCategories = db.GetAllCategoriesByUser(user);

            return View(allCategories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult CreateCategory()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(CreateCategoryViewModel m)
        {
            try
            {
                DB db = new DB();

                var userName = User.FindFirstValue(ClaimTypes.Name);
                User user = db.GetUserByUserName(userName);

                if (db.ExistCategoryName(m.CategoryName))
                {
                    ViewData["Category"] = m.CategoryName;
                    return View("CategoryAlreadyExists");
                }

                else
                {
                    Category category = new Category();
                    category.Name = m.CategoryName;
                    category.UserId = user.Id;
                    db.CreateCategory(category);

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
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

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
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
