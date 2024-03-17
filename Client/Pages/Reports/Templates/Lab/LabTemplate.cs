using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.Models.Reports;

namespace Client.Pages.Reports.Templates.Lab;

public class LabTemplate(ReportData Model)
{

    public async ValueTask<byte[]> Create()
    {
        return await Task.Run(() =>
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginTop(0, Unit.Inch);
                    page.Header().Component(new HeaderContent(Model!.ReportHeader!));
                    page.Content().Section("Content").Component(new ReportContent(Model));
                    page.Footer().Component(new FooterContent(Model!.ReportFooter!));

                });
            }).GeneratePdf();
        });
    }
}
