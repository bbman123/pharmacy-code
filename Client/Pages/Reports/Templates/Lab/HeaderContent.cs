using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Models.Reports;

namespace Client.Pages.Reports.Templates.Lab
{
    public class HeaderContent(ReportHeader header) : IComponent
    {
        private int pictureSize = 25;
        public void Compose(IContainer container)
        {
            container.AlignLeft().Column(column =>
            {
                column.Item()
                      .AlignLeft()
                      .Width(pictureSize, Unit.Millimetre)
                      .Height(pictureSize, Unit.Millimetre)
                      .Image(header!.Logo!).FitArea();

                column.Item()
                      .AlignLeft()
                      .Text($"{header!.Store!.BranchAddress}")
                      .FontSize(15);

                column.Item()
                      .AlignLeft()
                      .Text($"{header!.Store.PhoneNo1}")
                      .FontSize(15);
                
                column.Item()
                      .AlignLeft()
                      .Text($"{header!.Title}")
                      .FontSize(15);
                //column.Item().AlignCenter().Text($"Receipt {Model!.Order!.ReceiptNo.ToString().PadLeft(4, '0')}").FontSize(8);
            });
        }
    }
}
