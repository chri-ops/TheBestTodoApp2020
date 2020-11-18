﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library;
using TheBestTodoApp2020.Models;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TheBestTodoApp2020.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        // GET: TodoController
        public ActionResult Index()
        {
            DB db = new DB();
            TodoIndexViewModel m = new TodoIndexViewModel();

            var userName = User.FindFirstValue(ClaimTypes.Name);

            User user = db.FindLoggedInUser(userName);

            if (user == null)
            {
                return View("NoLoggedInUser");
            }

            else
            {
                m.User = user;
                m.TodoLists = db.GetAllTodoListsForUser(m.User);

                foreach (TodoList todoList in m.TodoLists)
                {
                    bool allDone = true;

                    //List<Todo> todos = db.GetTodosForTodoList(todoList);

                    List<Todo> todos = db.GetAllTodosForTodoList(todoList.Id);

                    todoList.AmountOfTodos = todos.Count; // Sets the amount of todo's for list into list prop.
                    todoList.AmountOfDoneTodos = 0; // Presets the counter to zero.


                    foreach (Todo todo in todos)
                    {
                        if (todo.Done == false)
                        {
                            allDone = false;
                        }
                        else
                        {
                            todoList.AmountOfDoneTodos++; // for every done todo, add to the amount of done todos-prop in todolist.
                        }
                    }

                    if (allDone)
                    {
                        todoList.AllDone = true;
                    }
                    else
                    {
                        todoList.AllDone = false;
                    }

                    db.UpdateTodoList(todoList);
                }

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
            DB db = new DB();

            CreateTodoListViewModel m = new CreateTodoListViewModel();

            var userName = User.FindFirstValue(ClaimTypes.Name);

            User user = db.GetUserByUserName(userName);

            m.AllCategories = db.GetAllCategoriesByUser(user);

            return View(m);
        }

        // POST: TodoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTodoList(CreateTodoListViewModel m)
        {
            try
            {
                DB db = new DB();
                Category category = db.GetCategoryByName(m.CategoryName);

                TodoList todoList = new TodoList();
                todoList.Title = m.Title;
                todoList.Category = category;

                var userName = User.FindFirstValue(ClaimTypes.Name);

                db.AddTodoList(todoList, userName);

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

            todo.Comment = m.Comment;
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


        public ActionResult EditTodo(string todoId)
        {
            DB db = new DB();

            var id = ObjectId.Parse(todoId);

            Todo todo = db.GetTodoById(id);

            EditTodoViewModel m = new EditTodoViewModel();

            m.Comment = todo.Comment;
            m.Description = todo.Description;
            m.Done = todo.Done;
            m.Id = todo.Id.ToString();
            m.TodoListId = todo.TodoListId.ToString();

            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTodo(EditTodoViewModel m)
        {
            DB db = new DB();

            Todo todo = new Todo();

            todo.Comment = m.Comment;
            todo.Description = m.Description;
            todo.Done = m.Done;

            todo.Id = ObjectId.Parse(m.Id);
            todo.TodoListId = ObjectId.Parse(m.TodoListId);

            db.UpdateTodo(todo);

            // View for todoList for current Todo..

            return RedirectToAction("EditTodoList", new { todoListId = todo.TodoListId.ToString() });
        }


        // GET: TodoController/Edit/5
        public ActionResult EditTodoList(string todoListId)
        {
            //EditTodoListViewModel m = new EditTodoListViewModel();

            //m.TodoListId = todoListId;

            var id = ObjectId.Parse(todoListId);
            
            DB db = new DB();

            TodoList todoList = db.GetTodoListById(id);

            ViewData["TodoListId"] = todoList.Id.ToString();
            ViewData["TodoListTitle"] = todoList.Title.ToString();
            if (todoList.Category != null) ViewData["Category"] = " [ " + todoList.Category.Name + " ]";
            else ViewData["Category"] = "";

            List<Todo> listOfTodos = db.GetAllTodosForTodoList(id);

            return View(listOfTodos);
        }

        public IActionResult EditTodoListTitle(string todoListId)
        {
            DB db = new DB();

            var id = ObjectId.Parse(todoListId);
            TodoList todoList = db.GetTodoListById(id);

            var userName = User.FindFirstValue(ClaimTypes.Name);

            User user = db.GetUserByUserName(userName);

            ViewData["TodoListId"] = todoList.Id.ToString();
            ViewData["TodoListTitle"] = todoList.Title.ToString();

            EditTodoListTitleViewModel m = new EditTodoListTitleViewModel();

            m.Title = todoList.Title;
            m.TodoListId = todoList.Id.ToString();

            if (todoList.Category == null || String.IsNullOrEmpty(todoList.Category.Name)) m.CategoryName = "";
            else m.CategoryName = todoList.Category.Name;
            
            m.AllCategories = db.GetAllCategoriesByUser(user);

            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTodoListTitle(EditTodoListTitleViewModel m)
        {
            DB db = new DB();

            var id = ObjectId.Parse(m.TodoListId);

            TodoList todoList = db.GetTodoListById(id);

            todoList.Title = m.Title;

            if (String.IsNullOrEmpty(m.CategoryName)) todoList.Category = null;
            else todoList.Category = db.GetCategoryByName(m.CategoryName);

            db.UpdateTodoList(todoList);

            return RedirectToAction("EditTodoList", new { todoListId = todoList.Id.ToString() });
        }

        public ActionResult CheckTodo(string todoId)
        {
            var id = ObjectId.Parse(todoId);

            DB db = new DB();

            Todo todo = db.GetTodoById(id);

            //CheckTodoViewModel m = new CheckTodoViewModel();

            //m.TodoId = todo.Id.ToString();
            //m.Description = todo.Description;
            //m.Done = todo.Done;

            //Checking/unchecking the Done-box
            if (todo.Done == true) todo.Done = false;
            else todo.Done = true;

            db.UpdateTodo(todo);

            return RedirectToAction("EditTodoList", new { todoListId = todo.TodoListId.ToString() });
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
        public ActionResult DeleteTodoList(string todoListId)
        {
            var id = ObjectId.Parse(todoListId);

            DB db = new DB();

            TodoList todoList = db.GetTodoListById(id);

            db.DeleteTodoList(todoList);

            return RedirectToAction("Index", "Todo");
        }

        // POST: TodoController/Delete/5
        
        public ActionResult DeleteTodo(string todoId, string todoListId)
        {
            try
            {
                DB db = new DB();

                var id = ObjectId.Parse(todoId);

                db.DeleteTodo(id);

                return RedirectToAction("EditTodoList", new { todoListId = todoListId });
            }
            catch
            {
                return View();
            }
        }
    }
}
