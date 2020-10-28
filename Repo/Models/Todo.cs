using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    public class Todo
    {
        public ObjectId Id { get; set; }
        public string Description { get; set; }

        public bool Done { get; set; }
        //public DateTime ToBeDone { get; set; }

        public string Comment { get; set; }
        public ObjectId TodoListId { get; set; }
    }
}
