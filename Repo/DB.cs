using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Library.Models;

namespace Library
{
    public class DB
    {
        private readonly IMongoDatabase _db;

        // private const string TODOS = "todos";

        private IMongoCollection<Todo> _todos;
        private const string _TODOS = "todos";

        private IMongoCollection<TodoList> _todoLists;
        private const string _TODOLISTS = "todolists";

        private IMongoCollection<User> _users;
        private const string _USERS = "users";

        public DB()
        {
            MongoClient client = new MongoClient();

            _db = client.GetDatabase("thebesttodoapp2020");

            _todos = _db.GetCollection<Todo>(_TODOS);

            _todoLists = _db.GetCollection<TodoList>(_TODOLISTS);

            _users = _db.GetCollection<User>(_USERS);
        }

        public List<TodoList> GetAllTodoListsForUser(User user)
        {
            var todoListsForUser = _todoLists.Find(t => t.UserId == user.Id).ToList();

            return todoListsForUser;
        }

        public bool UserNameExists(string userName)
        {
            if (_users.Find(u => u.UserName == userName).FirstOrDefault() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Todo> GetAllTodos(ObjectId todoListId)
        {
            var result = _todos.Find(t => t.TodoListId == todoListId);

            return result.ToList();
        }

        public void CreateUser(User user)
        {
            user.IsLoggedIn = false;
            _users.InsertOne(user);
        }

        public void AddTodoList(TodoList todoList)
        {
            User user = FindLoggedInUser();

            todoList.UserId = user.Id;

            _todoLists.InsertOne(todoList);
        }

        public void LogoutAll()
        {
            var loggedInUsers = _users.Find(u => u.IsLoggedIn == true).ToList();

            var update = Builders<User>.Update.Set(u => u.IsLoggedIn, false);

            foreach (User user in loggedInUsers)
            {
                _users.UpdateOne(u => u.Id == user.Id, update);
            }
        }

        public User FindLoggedInUser()
        {
            User loggedInUser = _users.Find(u => u.IsLoggedIn == true).FirstOrDefault();

            return loggedInUser;
        }

        public void AddTodo(Todo todo)
        {
            _todos.InsertOne(todo);
        }

        public User GetUserByUserName(string userName)
        {
            User user = _users.Find(u => u.UserName == userName).FirstOrDefault();

            return user;
        }

        public void LoginUser(User user)
        {
            LogoutAll();

            var update = Builders<User>.Update.Set(u => u.IsLoggedIn, true);

            _users.UpdateOne(u => u.Id == user.Id, update);
        }

        public bool LogoutUser()
        {
            User loggedInUser = FindLoggedInUser();

            if (loggedInUser != null)
            {
                var update = Builders<User>.Update.Set(u => u.IsLoggedIn, false);

                _users.UpdateOne(u => u.Id == loggedInUser.Id, update);

                return true;
            }

            else
            {
                return false;
            }
        }

        public TodoList GetTodoListById(ObjectId id)
        {
            TodoList todoList = _todoLists.Find(t => t.Id == id).FirstOrDefault();

            return todoList;
        }

        public Todo GetTodoById(ObjectId id)
        {
            Todo todo = _todos.Find(t => t.Id == id).FirstOrDefault();

            return todo;
        }

        public void UpdateTodo(Todo todo)
        {
            var update = Builders<Todo>.Update
                .Set(t => t.Comment, todo.Comment)
                .Set(t => t.Description, todo.Description)
                .Set(t => t.Done, todo.Done);

            _todos.UpdateOne(t => t.Id == todo.Id, update);
        }
    }
}