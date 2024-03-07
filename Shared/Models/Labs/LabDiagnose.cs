using Shared.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Labs;

public class LabDiagnose
{
    [Required] public Guid Id { get;set; }
    public Guid LabOrderId { get; set; }
    public Guid LabTestId { get; set; }
    public Guid LabScientistId { get; set; }
    [Column(TypeName = "jsonb")]
    public List<LabTestResult> Results { get; set; } =new List<LabTestResult>();
    [ForeignKey(nameof(LabOrderId))]
    public virtual LabOrder? Order { get; set; }
    [ForeignKey(nameof(LabTestId))]
    public virtual LabTest? Test { get; set; }
    [ForeignKey(nameof(LabScientistId))]
    public virtual User? LabScientist { get; set; }
}
