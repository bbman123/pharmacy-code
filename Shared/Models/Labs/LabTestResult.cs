using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Labs;
[Keyless]
public class LabTestResult
{
    [Required]
    public string? TestName { get; set; }
    [Required]
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? Range { get; set; }
}
