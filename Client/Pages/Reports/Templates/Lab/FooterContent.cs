using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Models.Reports;

namespace Client.Pages.Reports.Templates.Lab
{
    public class FooterContent(ReportFooter footer) : IComponent
    {
        private int pictureSize = 25;
        public void Compose(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Width(pictureSize, Unit.Millimetre).Height(pictureSize, Unit.Millimetre).Image(footer!.QR!).FitArea();
                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Item()
                          .AlignRight()
                          .Text(text =>
                          {
                              text.Span("_____________________________");
                          });
                    column.Item()
                          .AlignCenter()
                          .Text(text =>
                          {
                              text.Span(footer.User!.ToString());
                          });
                    column.Item()
                          .AlignCenter()
                          .Text(text =>
                          {
                              text.Span("Lab Scientist");
                          });
                });
            });
        }
    }
}
