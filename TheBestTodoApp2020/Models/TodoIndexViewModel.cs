using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBestTodoApp2020.Models
{
    public class TodoIndexViewModel
    {
        public User User { get; set; }

        public List<TodoList> TodoLists { get; set; }

        public TodoIndexViewModel()
        {
            TodoLists = new List<TodoList>();
        }
    }
}
