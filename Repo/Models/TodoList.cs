using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    public class TodoList
    {
        public string Title { get; set; }
        public List<Todo> Todos { get; set; }

        public bool AllDone { get; set; }

        public TodoList()
        {
            Todos = new List<Todo>();
        }
    }
}
