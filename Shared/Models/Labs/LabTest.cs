﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;

namespace Shared.Models.Labs;

public class LabTest
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required!")]
    public string? TestName { get; set; }        
    [Column(TypeName = "decimal(18, 2)")]
    [Required(ErrorMessage = "Rate is required!")]
    public decimal Rate { get; set; }
    public ServiceType Type { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
    [Column(TypeName = "jsonb")]
    public List<LabTestResult>? Findings { get; set; } = new();
}
