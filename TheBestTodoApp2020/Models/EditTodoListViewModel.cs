using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;

namespace TheBestTodoApp2020.Models
{
    public class EditTodoListViewModel
    {
        public ObjectId TodoListId { get; set; }
        public List<Todo> listOfTodos { get; set; }

        public string TodoListTitle { get; set; }

        public EditTodoListViewModel()
        {
            listOfTodos = new List<Todo>();
        }
    }
}
