using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public class OrderReferer
    {
        [Required]
        public Guid Id { get; set; }
        public Guid OrderId {  get; set; }
        public Guid LabOrderId {  get; set; }
        public Guid RefererId { get; set; }
        public decimal Price { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
    }
}
