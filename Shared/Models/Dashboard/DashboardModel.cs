using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard
{
    public class DashboardModel
    {
        public int TotalBranches { get; set; }
        public int TotalSales => TotalPharmacySales + TotalLabSales;
        public int TotalPharmacySales { get; set; }
        public int TotalLabSales { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalEmployees { get; set; }
        public OrderChartModel[] ServicePieChart { get; set; } = [];
        public OrderChartModel[] ProductPieChart { get; set; } = [];
        public OrderSalesLine[] ServiceSales { get; set; } = [];
        public OrderSalesLine[] ProductSales { get; set; } = [];
        public BranchSalesChart[] BranchSales { get; set;  }  = []; 
        public BranchNetSalesChart[] BranchNetSales { get; set;  }  = []; 
        public Dictionary<string, int>? ServiceTopCustomer { get; set; }
        public Dictionary<string, int>? ProductTopCustomer { get; set; }
        public bool IsBusy { get; set; }
    }

    public class OrderSalesLine
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int? Sales { get; set; }
    }
    public class OrderChartModel
    {
        public string? Item { get; set; }
        public int SalesCount { get; set; }
    }

    public class BranchSalesChart
    {
        public string? Branch { get; set; }
        public int PharmacySales { get; set; }
        public int LabSales { get; set; }
    }
    
    public class BranchNetSalesChart
    {
        public string? Branch { get; set; }
        public decimal PharmacySales { get; set; }
        public decimal LabSales { get; set; }
    }
}
