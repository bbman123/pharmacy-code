﻿using Shared.Models.Labs;
using Shared.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers;

public class OrderCartRow
{
    public decimal GrandTotal { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost => SelectedOption == "Service" ? Service!.Rate : Product!.UnitPrice;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total => Quantity * Cost;
    public Product? Product { get; set; } = new();
    public LabTest? Service { get; set; } = new();
    public string? ItemName => SelectedOption == "Service" ? Service!.TestName : Product!.ProductName;
    public string? SearchByBarcode { get; set; }
    public string? SelectedOption { get; set; }
    public DateTime? DeliveryDate { get; set; }
}
