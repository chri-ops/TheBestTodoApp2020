﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBestTodoApp2020.Models
{
    public class CreateTodoListViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
