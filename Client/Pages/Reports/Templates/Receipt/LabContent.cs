using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using Shared.Models.Labs;
using System.Reflection;
using Shared.Models.Customers;

namespace Client.Pages.Reports.Templates.Receipt
{
    public class LabContent(LabOrder Order) : IComponent
    {
        public void Compose(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Item().Row(row =>
                {                    
                    row.RelativeItem().AlignRight().Text($"Date: {Order!.OrderDate.ToString("dd/MM/yyyy")}").FontSize(8);
                    row.RelativeItem().AlignRight().Text($"#: {Order!.ReceiptNo.ToString().PadLeft(4, '0')}").FontSize(8);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text($"Name: {Order!.Customer!.CustomerName}").FontSize(8);
                });
                column.Spacing(8);                
            });
        }
    }
}
