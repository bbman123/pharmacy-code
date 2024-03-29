﻿using Shared.Enums;
using Shared.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Orders
{
    public class OrderWithData
    {
        public Guid Id { get; set; }
        public string? StoreName { get; set; }
        public string? OrderType { get; set; }
        public int ReceiptNo { get; set; }
        public DateOnly Date { get; set; }        
        public string? CustomerName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Discount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SubTotal => TotalAmount - Discount;
        public TestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
