﻿using System.ComponentModel.DataAnnotations;

namespace MongoTest.DTO
{
    public class CategoryDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
    }
}