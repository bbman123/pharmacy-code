using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Labs;
[Keyless]
public class LabTestResult
{
    public string? TestName { get; set; }
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? Range { get; set; }
}
