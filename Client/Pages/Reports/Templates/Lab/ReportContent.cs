using MudBlazor;
using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Models.Orders;
using Shared.Models.Reports;

namespace Client.Pages.Reports.Templates.Lab
{
    
    public class ReportContent(ReportData Model) : IComponent
    {
        [Obsolete]
        public void Compose(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                var Order = Model!.LabOrder!;
                column.Item().Grid(grid =>
                {
                    grid.Spacing(2.5f);
                    grid.Item(6).AlignLeft().Text($"Name: {Order!.Customer!.CustomerName}").FontSize(12);
                    grid.Item(6).AlignRight().Text($"#: {Order!.ReceiptNo.ToString().PadLeft(4, '0')}").FontSize(12);
                    
                    grid.Item(6).AlignLeft().Text($"Gender: {Order!.Customer!.CustomerName}").FontSize(12);
                    grid.Item(6).AlignRight().Text($"Date: {Order!.OrderDate:dd/MM/yyyy}").FontSize(12);

                    grid.Item(6).AlignLeft().Text($"Age: ").FontSize(12);
                    grid.Item(6).AlignRight().Text($"Consultation: {Order!.Consultation:N2}").FontSize(12);
                    
                    grid.Item(6).AlignLeft().Text($"Phone No: {Order!.Customer!.PhoneNo} ").FontSize(12);
                    grid.Item(6).AlignRight().Text($"Total Amount: {Order!.Total:N2}").FontSize(12);

                });                
                column.Spacing(8);
                var dignosis = Order.Diagonses!.ToList();
                var items = Order!.Items!.ToList();
                column.Item().Component(new DiagnosisTable(dignosis, items));
            });
        }
    }
}
