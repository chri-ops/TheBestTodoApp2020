using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    public class Todo
    {
        public ObjectId id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime TBD { get; set; }

        public ObjectId TodoListId { get; set; }
    }
}
