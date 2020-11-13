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

        private IMongoCollection<Student> _students;
        private const string _STUDENTS = "students";

        public DB()
        {
            //MongoClient client = new MongoClient();

            MongoClient client = new MongoClient("mongodb+srv://krishanromeda:Charliewasourdog1982@cluster0.bhta4.mongodb.net/thebesttodoapp2020?retryWrites=true&w=majority");

            _db = client.GetDatabase("thebesttodoapp2020");

            _todos = _db.GetCollection<Todo>(_TODOS);

            _todoLists = _db.GetCollection<TodoList>(_TODOLISTS);

            _users = _db.GetCollection<User>(_USERS);

            _students = _db.GetCollection<Student>(_STUDENTS);
        }

        public List<TodoList> GetAllTodoListsForUser(User user)
        {
            var todoListsForUser = _todoLists.Find(t => t.UserId == user.Id).ToList();

            return todoListsForUser;
        }

        //public List<Todo> GetTodosForTodoList(TodoList todoList)
        //{
        //    List<Todo> todos = _todos.Find(t => t.TodoListId == todoList.Id).ToList();

        //    return todos;
        //}

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

        public void UpdateTodoList(TodoList todoList)
        {
            var update = Builders<TodoList>.Update.
                Set(t => t.AllDone, todoList.AllDone).
                Set(t => t.Title, todoList.Title).
                Set(t => t.ToBeDone, todoList.ToBeDone).
                //Set(t => t.Todos, todoList.Todos).
                Set(t => t.UserId, todoList.UserId);

            _todoLists.UpdateOne(t => t.Id == todoList.Id, update);
        }

        public List<Todo> GetAllTodosForTodoList(ObjectId todoListId)
        {
            var result = _todos.Find(t => t.TodoListId == todoListId);

            return result.ToList();
        }

        public void CreateUser(User user)
        {
            user.IsLoggedIn = false;
            _users.InsertOne(user);
        }

        public void AddTodoList(TodoList todoList, string userName)
        {
            User user = FindLoggedInUser(userName);

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

        public User FindLoggedInUser(string userName)
        {
            User loggedInUser = _users.Find(u => u.UserName == userName).FirstOrDefault();

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
            // LogoutAll(); // edited out

            var update = Builders<User>.Update.Set(u => u.IsLoggedIn, true);

            _users.UpdateOne(u => u.Id == user.Id, update);
        }

        public bool LogoutUser(string userName)
        {
            User loggedInUser = FindLoggedInUser(userName);

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

        public void DeleteTodoList(TodoList todoList)
        {
            _todoLists.DeleteOne(t => t.Id == todoList.Id);

            List<Todo> allTodosForList = GetAllTodosForTodoList(todoList.Id);

            foreach (Todo todo in allTodosForList)
            {
                _todos.DeleteOne(t => t.Id == todo.Id);
            }
        }

        public void DeleteTodo(ObjectId todoId)
        {
            _todos.DeleteOne(t => t.Id == todoId);
        }
    }
}