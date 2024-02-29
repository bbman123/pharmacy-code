﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Company
{
    public class Store
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Name is required")]
        public string? BranchName { get; set; } = "Mujahid Pharmacy";
        public string? BranchAddress { get; set; }
        [Required(ErrorMessage = "Phone No is required")]
        public string? PhoneNo1 { get; set; }
        public string? PhoneNo2 { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
    }
}
