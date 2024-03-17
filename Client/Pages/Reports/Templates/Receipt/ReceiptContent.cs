using MudBlazor;
using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Models.Orders;
using Shared.Models.Reports;

namespace Client.Pages.Reports.Templates.Receipt
{
    
    public class ReportContent(ReportData Model) : IComponent
    {
        List<OrderItemDetail> Details = new();
        public void Compose(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                if (Model!.ReportType == "Lab")
                {
                    
                    var Customer = Model!.Customer!;
                    var Order = Model!.LabOrder!;
                    Order!.Customer = Customer;
                    Details = Model!.LabOrder!.Items!.GroupBy(x => x.ServiceId, (x, y) => new OrderItemDetail
                    {
                        Quantity = 1,
                        ItemName = y.Select(c => c.ServiceName).FirstOrDefault(),
                        Cost = y.Sum(z => z.Cost),
                        Consultation = Order.Consultation,
                        ConsultationNote = Order.ConsultationNote,
                    }).ToList();
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text($"Date: {Order!.OrderDate.ToString("dd/MM/yyyy")}").FontSize(8);
                        row.RelativeItem().AlignRight().Text($"#: {Order!.ReceiptNo.ToString().PadLeft(4, '0')}").FontSize(8);
                    });
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text($"Name: {Order!.Customer!.CustomerName}").FontSize(8);
                    });                    
                }
                else
                {
                    var Order = Model!.Order!;
                    Details = Model!.Order!.ProductOrders.GroupBy(x => x.ProductId, (x, y) => new OrderItemDetail
                    {
                        Quantity = y.Select(z => z.Quantity).First(),
                        ItemName = y.Select(c => c.Product).FirstOrDefault(),
                        Cost = y.Select(z => z.Cost).First(),
                        Consultation = Order.Consultation,
                        ConsultationNote = Order.ConsultationNote,
                    }).ToList();
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text($"#: {Order!.ReceiptNo.ToString().PadLeft(4, '0')}").FontSize(8);
                    });
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text($"Date: {Order!.OrderDate.ToString("dd/MM/yyyy")}").FontSize(8);
                    });                    
                }                
                column.Spacing(8);
                column.Item().Component(new ReceiptTable(Details));
            });
        }
    }
}
