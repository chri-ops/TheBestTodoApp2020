using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    public class TodoList
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public List<Todo> Todos { get; set; }

        public DateTime ToBeDone { get; set; }

        public bool AllDone { get; set; }

        public ObjectId UserId { get; set; }

        public TodoList()
        {
            Todos = new List<Todo>();
        }
    }
}
