﻿using System.ComponentModel.DataAnnotations;

namespace Puppy_Project.Models.OrderDTO
{
    public class inputOrderDTO
    {
        [Required]
        public int User_Id { get; set; }
        [Required]
        public int Product_Id { get; set; }
        [Required]
        public int Qty { get; set; }
    }
}