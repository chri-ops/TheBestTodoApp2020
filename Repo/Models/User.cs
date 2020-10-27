using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Library.Models
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsLoggedIn { get; set; }
        public List<TodoList> TodoLists { get; set; }

        public User()
        {
            TodoLists = new List<TodoList>();
            IsLoggedIn = false;
        }
    }
}
