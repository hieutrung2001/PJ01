﻿using PJ01.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PJ01.Core.ViewModels.Requests.Classes
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public List<StudentClass> StudentClasses { get; set; }
        public List<int> StudentSelectList { get; set; }
    }
}
