using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Helpers;
using Shared.Models.Orders;
using Shared.Models.Reports;

namespace Client.Pages.Reports.Templates.Receipt;

public class ReceiptTemplate(ReportData? model)
{
    private ReportData? Model { get; set; }
    public async ValueTask<byte[]> Create()
    {        
        Model = model;        
        return await Task.Run(() =>
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(3.1486f, 5.5f, Unit.Inch);
                    page.MarginTop(0, Unit.Inch);                    
                    page.Header().Component(new HeaderContent(Model!.ReportHeader!));                    
                    page.Content().Section("Content").Component(new ReportContent(Model));                    
                    page.Footer().Component(new FooterContent(Model!.ReportFooter!));

                });
            }).GeneratePdf();
        });
    }
}
