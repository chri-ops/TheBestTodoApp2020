using MongoDB.Bson;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBestTodoApp2020.Models
{
    public class CheckTodoViewModel
    {
        public string TodoId { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public bool Done { get; set; }
    }
}
