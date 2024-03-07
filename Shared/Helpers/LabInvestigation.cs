using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;

namespace Shared.Helpers;

public class LabInvestigation
{
    public Guid Id { get; set; }
    public Guid LabOrderId { get; set; }
    public Guid LabTestId { get; set; }
    public Guid LabScientistId { get; set; }
    public string? TestName { get; set; }
    public string? ScientistName { get; set; }
    public TestStatus Status { get; set; }

}
