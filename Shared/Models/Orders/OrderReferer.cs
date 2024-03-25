using Shared.Enums;
using Shared.Models.Labs;
using Shared.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Orders
{
    public class OrderReferer
    {
        [Required]
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? LabOrderId { get; set; }
        public Guid RefererId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
        public Referer? Referer { get; set; }
        [ForeignKey(nameof(OrderId))]
        public PharmacyOrder? PharmacyOrder { get; set; }
        [ForeignKey(nameof(LabOrderId))]
        public LabOrder? LabOrder { get; set; }
    }
}
