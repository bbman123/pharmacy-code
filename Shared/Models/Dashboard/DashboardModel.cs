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
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalEmployees { get; set; }
        public OrderChartModel[] ServicePieChart { get; set; } = Array.Empty<OrderChartModel>();
        public OrderChartModel[] ProductPieChart { get; set; } = Array.Empty<OrderChartModel>();
        public OrderSalesLine[] ServiceSales { get; set; } = Array.Empty<OrderSalesLine>();
        public OrderSalesLine[] ProductSales { get; set; } = Array.Empty<OrderSalesLine>();
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
}
