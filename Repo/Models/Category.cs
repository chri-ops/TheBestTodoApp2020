using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    public class Category
    {
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public string Name { get; set; }
    }
}
