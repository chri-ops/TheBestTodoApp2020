using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using System.ComponentModel.DataAnnotations;

namespace TheBestTodoApp2020.Models
{
    public class CreateTodoViewModel
    {
        public string TodoListId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        //public DateTime ToBeDone { get; set; }
        public bool Done { get; set; }
        public string Comment { get; set; }
    }
}
