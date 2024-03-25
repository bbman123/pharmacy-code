using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Client.Pages.Reports;

public class BillReceipt(byte[] Image, string? Type)
{
    public byte[] Create()
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(2.5f, 2.5f, Unit.Inch);
                page.MarginTop(0, Unit.Inch);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

            });
        });
        return document.GeneratePdf();
    }
    void ComposeHeader(IContainer container)
    {
        container.AlignCenter().Column(column =>
        {
            column.Item().AlignCenter().Text("Mujahid Pharmacy").ExtraBold().FontSize(15);
            column.Item().AlignCenter().Text($"{Type} Sale Receipt").Bold().FontSize(10);
            column.Item().AlignCenter().Text("Please present this to the Cashier").FontSize(10);
        });

    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(15).Column(column =>
        {
            column.Item()
                  .AlignCenter()
                  .Width(30, Unit.Millimetre)
                  .Height(30, Unit.Millimetre)
                  .AlignCenter()
                  .Image(Image)
                  .FitArea();            
        });
    }
}
