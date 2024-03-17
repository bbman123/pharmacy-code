using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Models.Labs;
using static MudBlazor.CategoryTypes;

namespace Client.Pages.Reports.Templates.Lab;

public class DiagnosisTable(List<LabDiagnose> diagnoses, List<LabOrderItem> items) : IComponent
{
    [Obsolete]
    public void Compose(IContainer container)
    {
        var services = diagnoses.GroupBy(x => x.LabTestId).Select(x => new
        {
            id = x.Key
        });
        container.PaddingVertical(1.2f).Padding(1.2f).Grid(grid =>
        {
            grid.Columns(12);
            grid.Item(3).Border(1).BorderColor("#000000").Background("#000000").Text("Test Name").FontColor("#FFFFFF").FontSize(10);
            grid.Item(3).Border(1).BorderColor("#000000").Background("#000000").Text("Result").FontColor("#FFFFFF").FontSize(10);
            grid.Item(3).Border(1).BorderColor("#000000").Background("#000000").Text("Unit").FontColor("#FFFFFF").FontSize(10);
            grid.Item(3).Border(1).BorderColor("#000000").Background("#000000").Text("Range").FontColor("#FFFFFF").FontSize(10);


            foreach (var service in services)
            {
                grid.Item(12).Text(items.FirstOrDefault(x => x.ServiceId == service.id)!.ServiceName).Bold().FontSize(10);
                grid.Spacing(2.5f);
                var results = diagnoses.Where(x => x.LabTestId == service.id).SelectMany(x => x.Results);
                foreach (var result in results)
                {
                    grid.Item(3).Border(1).BorderColor("#D2D2D2").Text(result.TestName).FontSize(10);
                    grid.Item(3).Border(1).BorderColor("#D2D2D2").Text(result.Result).FontSize(10);
                    grid.Item(3).Border(1).BorderColor("#D2D2D2").Text(result.Unit).FontSize(10);
                    grid.Item(3).Border(1).BorderColor("#D2D2D2").Text(result.Range).FontSize(10);
                }
            }
        });
        //container.PaddingVertical(1.2f).Padding(1.2f).Table(table =>
        //{
        //    table.ColumnsDefinition(column =>
        //    {
        //        column.RelativeColumn(2.5f);
        //        column.RelativeColumn(1f);
        //        column.RelativeColumn(1f);
        //        column.RelativeColumn(1f);
        //    });

        //    table.Header(header =>
        //    {
        //        header.Cell().AlignLeft().Border(1).BorderColor("#000000").Background("#000000").Text("Test Name").FontColor("#FFFFFF").FontSize(10);
        //        header.Cell().AlignLeft().Border(1).BorderColor("#000000").Background("#000000").Text("Result").FontColor("#FFFFFF").FontSize(10);
        //        header.Cell().AlignLeft().Border(1).BorderColor("#000000").Background("#000000").Text("Unit").FontColor("#FFFFFF").FontSize(10);
        //        header.Cell().AlignLeft().Border(1).BorderColor("#000000").Background("#000000").Text("Range").FontColor("#FFFFFF").FontSize(10);
        //    });

        //    foreach (var service in services)
        //    {
        //        table.Cell().ColumnSpan(4).RowSpan(1);
        //        table.Cell().ColumnSpan(4).AlignLeft().Text(text =>
        //        {
        //            text.Span(items.FirstOrDefault(x => x.ServiceId == service.id)!.ServiceName).Bold().FontSize(10);
        //        });
        //        var results = diagnoses.Where(x => x.LabTestId == service.id).SelectMany(x => x.Results);
        //        foreach (var result in results)
        //        {
        //            table.Cell().AlignLeft().Text(text =>
        //            {
        //                text.Span(result.TestName!.ToString()).FontSize(8);
        //            });
        //            table.Cell().AlignLeft().Text(text =>
        //            {
        //                text.Span(result.Result!.ToString()).FontSize(8);
        //            });
        //            table.Cell().AlignLeft().Text(text =>
        //            {
        //                text.Span(result.Unit!.ToString()).FontSize(8);
        //            });
        //            table.Cell().AlignLeft().Text(text =>
        //            {
        //                text.Span(result.Range!.ToString()).FontSize(8);
        //            });
        //        }
        //    }
        //});
    }

}

