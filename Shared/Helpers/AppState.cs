

using Shared.Models;
using Shared.Helpers;
using Shared.Models.Reports;
using Shared.Models.Products;

namespace Shared.Models;
public class AppState
{
   
    public event EventHandler? OnUpdateLayout;
    public void UpdateLayout() => OnUpdateLayout?.Invoke(this, EventArgs.Empty);
    public object? SelectedOption { get; set; }
    public bool IsProcessing { get; set; }
    public bool IsBusy { get; set; }
	public bool Entry { get; set; }
	public bool DialogVisibility { get; set; }
	public string? _searchString {get; set;}
    public bool IsUploading { get; set; }    
    public ReportData? ReportDataModel { get; set; }
    public Product? Product { get; set; }
    public string GetQrCode() {
        Converter converter = new Converter();
        var code = ReportDataModel!.Order is not null ? ReportDataModel!.Order.Id : ReportDataModel!.Order!.Id;
        return Convert.ToBase64String(converter.ConvertImageToByte(code));
    }
}