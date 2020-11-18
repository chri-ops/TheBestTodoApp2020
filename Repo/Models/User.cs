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
        public string P { get; set; }
        public bool IsLoggedIn { get; set; }

        public User()
        {
            IsLoggedIn = false;
        }
    }
}
