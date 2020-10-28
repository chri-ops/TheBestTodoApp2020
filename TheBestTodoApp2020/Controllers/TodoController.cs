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
using MongoDB.Bson;

namespace TheBestTodoApp2020.Controllers
{
    public class TodoController : Controller
    {
        // GET: TodoController
        public ActionResult Index()
        {
            DB db = new DB();
            TodoIndexViewModel m = new TodoIndexViewModel();
            User user = db.FindLoggedInUser();

            if (user == null)
            {
                return View("NoLoggedInUser");
            }

            else
            {
                m.User = user;
                m.TodoLists = db.GetAllTodoListsForUser(m.User);

                return View(m);
            }
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

                db.AddTodoList(todoList);

                return RedirectToAction("CreateTodo", "Todo", new { todoListId = todoList.Id.ToString() });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateTodo(string todoListId)
        {
            CreateTodoViewModel m = new CreateTodoViewModel();
            // var id = ObjectId.Parse(todoListId);

            // In user input form, DateTime.Now will be default/preset (for easier/more logical picking)
            //DateTime now = DateTime.Now;
            
            // Trying to get rid of milliseconds part of datetime in View (by setting time to whole minutes (00))
            //now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 00, now.Kind);

            //m.ToBeDone = now;

            m.TodoListId = todoListId;

            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTodo(CreateTodoViewModel m)
        {
            Todo todo = new Todo();

            todo.Description = m.Description;
            todo.Done = m.Done;
            //todo.ToBeDone = m.ToBeDone;
            todo.TodoListId = ObjectId.Parse(m.TodoListId);

            DB db = new DB();

            db.AddTodo(todo);

            AddAnotherTodoViewModel a = new AddAnotherTodoViewModel();
            a.TodoListId = m.TodoListId;

            return View("AddAnotherTodo", a);
        }



        // GET: TodoController/Edit/5
        public ActionResult EditTodoList(string todoListId)
        {
            EditTodoListViewModel m = new EditTodoListViewModel();

            var id = ObjectId.Parse(todoListId);
            m.TodoListId = id;
            
            DB db = new DB();

            TodoList todoList = db.GetTodoListById(id);

            m.TodoListTitle = todoList.Title;
            m.listOfTodos = db.GetAllTodos(id);

            return View(m);
        }

        public ActionResult CheckTodo(string todoId)
        {
            var id = ObjectId.Parse(todoId);

            DB db = new DB();

            Todo todo = db.GetTodoById(id);

            CheckTodoViewModel m = new CheckTodoViewModel();

            m.TodoId = todo.Id.ToString();
            m.Description = todo.Description;
            m.Done = todo.Done;

            return View("CheckTodo", m);
        }

        // POST: TodoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckTodo(CheckTodoViewModel m)
        {
            try
            {
                DB db = new DB();

                var id = ObjectId.Parse(m.TodoId);

                Todo todo = db.GetTodoById(id);

                todo.Description = m.Description;
                todo.Comment = m.Comment;
                todo.Done = m.Done;

                db.UpdateTodo(todo);

                // Check if all todo's are done-marked: if so, AllDone should be marked on todoList
                // But then.. If we keep the possibility to uncheck checked todo's, the list has
                // to reflect that, again. Hmm....

                return RedirectToAction("EditTodoList", new { todoListId = todo.TodoListId.ToString() });
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
