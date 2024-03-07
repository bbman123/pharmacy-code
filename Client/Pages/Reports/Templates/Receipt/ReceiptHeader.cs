using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Models.Reports;

namespace Client.Pages.Reports.Templates.Receipt
{
    public class ReceiptHeader(ReportHeader header) : IComponent
    {
        private int pictureSize = 25;
        public void Compose(IContainer container)
        {
            container.AlignCenter().Column(column =>
            {
                column.Item()
                      .AlignCenter()
                      .Width(pictureSize, Unit.Millimetre)
                      .Height(pictureSize, Unit.Millimetre)
                      .Image(header!.Logo, ImageScaling.FitArea);

                column.Item()
                      .AlignCenter()
                      .Text($"{header!.Store!.BranchAddress}")
                      .FontSize(8);

                column.Item()
                      .AlignCenter()
                      .Text($"{header!.Store.PhoneNo1}")
                      .FontSize(8);
                
                column.Item()
                      .AlignCenter()
                      .Text($"{header!.Title}")
                      .FontSize(8);
                //column.Item().AlignCenter().Text($"Receipt {Model!.Order!.ReceiptNo.ToString().PadLeft(4, '0')}").FontSize(8);
            });
        }
    }
}
