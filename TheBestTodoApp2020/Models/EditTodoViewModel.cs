using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBestTodoApp2020.Models
{
    public class EditTodoViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Done { get; set; }
        public string Comment { get; set; }
        public string TodoListId { get; set; }
    }
}
