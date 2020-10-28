using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library;
using TheBestTodoApp2020.Models;
using Library.Models;
using Library;

namespace TheBestTodoApp2020.Controllers
{
    public class TodoController : Controller
    {
        // GET: TodoController
        public ActionResult Index()
        {
            DB db = new DB();

            User user = db.FindLoggedInUser();

            return View(user);
        }

        // GET: TodoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TodoController/Create
        public ActionResult CreateTodoList()
        {
            return View();
        }

        // POST: TodoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTodoList(CreateTodoListViewModel m)
        {
            try
            {
                TodoList todoList = new TodoList();
                todoList.Title = m.Title;

                DB db = new DB();

                db.AddTodoListToUser(todoList);

                return RedirectToAction("Index", "Todo");
            }
            catch
            {
                return View();
            }
        }

        // GET: TodoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TodoController/Edit/5
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

        // GET: TodoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TodoController/Delete/5
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
