using Shared.Models.Company;

namespace Shared.Models.Reports;
public class ReportHeader
{
    public Store? Store { get; set; }
    public byte[]? Logo { get; set; }
    public string? Title { get; set; }
}

public class ReportFooter
{
    public byte[]? QR { get; set; }
}