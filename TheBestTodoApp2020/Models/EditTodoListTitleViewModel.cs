﻿using Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBestTodoApp2020.Models
{
    public class EditTodoListTitleViewModel
    {
        public string TodoListId { get; set; }
        [Required]
        public string Title { get; set; }
        public string CategoryName { get; set; }

        public List<Category> AllCategories { get; set; } = new List<Category>();
    }
}
