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

        private IMongoCollection<User> _users;
        private const string _USERS = "users";

        public DB()
        {
            MongoClient client = new MongoClient();

            _db = client.GetDatabase("thebesttodoapp2020");

            _todos = _db.GetCollection<Todo>(_TODOS);

            _users = _db.GetCollection<User>(_USERS);
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

        public List<Todo> GetAllTodos()
        {
            var result = _todos.Find(t => true);

            return result.ToList();
        }

        public void CreateUser(User user)
        {
            user.IsLoggedIn = false;
            _users.InsertOne(user);
        }

        public void AddTodoListToUser(TodoList todoList)
        {
            User user = FindLoggedInUser();

            user.TodoLists.Add(todoList);

            var update = Builders<User>.Update.Set(u => u.TodoLists, user.TodoLists);

            _users.UpdateOne(u => u.Id == user.Id, update);
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
    }
}