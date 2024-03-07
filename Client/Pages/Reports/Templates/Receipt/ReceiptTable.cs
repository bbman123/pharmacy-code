using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Models.Orders;

namespace Client.Pages.Reports.Templates.Receipt
{
    public class ReceiptTable(List<OrderItemDetail> items) : IComponent
    {
        public void Compose(IContainer container)
        {
            container.PaddingVertical(1.2f).Padding(1.2f).Table(table =>
            {
                table.ColumnsDefinition(column =>
                {
                    column.RelativeColumn(0.35f);
                    column.RelativeColumn(2.5f);
                    column.RelativeColumn(1f);
                    // column.RelativeColumn(1f);
                });

                table.Header(header =>
                {
                    header.Cell().AlignCenter().Text("Qty").FontSize(8);
                    header.Cell().Text("Item").FontSize(8);
                    header.Cell().AlignRight().Text("Cost").FontSize(8);                    
                });

                foreach (var item in items)
                {
                    table.Cell().AlignCenter().Text(text =>
                    {
                        text.Span(item.Quantity.ToString()).FontSize(8);
                    });
                    table.Cell().Text(item.ItemName).FontSize(8);
                    table.Cell().AlignRight().Text($"{(item.Cost * (decimal)item.Quantity):N2}").FontSize(8);
                }
                table.Footer(footer => 
                {
                    
                    footer.Cell().RowSpan(3).ColumnSpan(3).Row(row =>
                    {
                        row.RelativeItem(7).AlignRight().Text("Total").FontSize(8);
                        row.RelativeItem(2).AlignRight().Text(GetTotal()).FontSize(8);
                    });
                    if (items.Any(x => x.Consultation > 0))
                    {
                        footer.Cell().RowSpan(3).ColumnSpan(3).Row(row =>
                        {
                            row.RelativeItem(7).AlignRight().Text("Consultation").FontSize(8);
                            row.RelativeItem(2).AlignRight().Text(GetConsultationFee()).FontSize(8);
                        });
                    }
                    footer.Cell().RowSpan(3).ColumnSpan(3).Row(row =>
                    {
                        row.RelativeItem(7).AlignRight().Text("Grand Total").FontSize(8);
                        row.RelativeItem(2).AlignRight().Text(GetGrandTotal()).FontSize(8);
                    });


                    if (items.Any(x => x.Consultation > 0))
                    {
                        footer.Cell().RowSpan(3).ColumnSpan(3).Column(row =>
                        {
                            row.Item().AlignLeft().Text(text =>
                            {
                                text.Span("Consultation Note").FontSize(9).Italic(true);
                            });
                            row.Item().AlignLeft().Text(text =>
                            {
                                text.Span(GetConsultationNote()).FontSize(8);
                            });
                        });
                    }      
                    
                });
            });
        }

        private string GetTotal() => items.Sum(x => (decimal)x.Quantity * x.Cost).ToString("N2");
        private string GetGrandTotal() => (items.Sum(x => (decimal)x.Quantity * x.Cost) + items.Select(x => x.Consultation).FirstOrDefault(0m)).ToString("N2");
        private string GetConsultationFee() => items.Select(x => x.Consultation).FirstOrDefault(0m).ToString("N2");
        private string GetConsultationNote() => items.Select(x => x.ConsultationNote!).FirstOrDefault(string.Empty);
    }
}
