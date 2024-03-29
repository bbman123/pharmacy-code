﻿using Client.Pages.Reports;
using Client.Pages.Reports.Templates.Lab;
using Client.Pages.Reports.Templates.Receipt;
using Microsoft.JSInterop;
using Shared.Helpers;
using Shared.Models.Labs;
using Shared.Models.Orders;
using Shared.Models.Reports;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Client.Services.Orders;

public interface IOrderService
{
    Task<bool> AddProductOrder(PharmacyOrder model);
    Task<bool> AddLabOrder(LabOrder model);
    Task<bool> UpdateLabOrder(LabOrder? model);
    Task<int> GetReceiptNo(string Type, Guid StoreID);
    Task<LabOrder?> GetLabOrder(Guid id);
    Task<LabOrder?> GetLabOrder(int receiptNumber);
    Task<PharmacyOrder?> GetPharmacyOrder(Guid id);
    Task<PharmacyOrder?> GetPharmacyOrder(int receiptNumber);
    Task<GridDataResponse<OrderWithData>?> GetOrderByStore(string Type, PaginationParameter parameter);
    List<LabOrderItem> LabOrderItems(Guid id, List<OrderCartRow> rows);
    List<ProductOrderItem> DrugOrderItems(Guid id, List<OrderCartRow> rows);
    ICollection<OrderReferer> GetOrderReferers(Guid id, string type, ICollection<OrderReferer> referers);
    Task GetReceipt(string Type, ReportData report);
    Task GetBillQrCode(Guid id, string Type);
    Task GetLabResults(Guid id);
}
public class OrderService : IOrderService
{
    private readonly IHttpClientFactory _client;
    private readonly IJSRuntime _js;
    public OrderService(IHttpClientFactory client, IJSRuntime js)
    {
        _client = client;
        _js = js;
    }


    public async Task<bool> AddLabOrder(LabOrder model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/laborders", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }
    
    public async Task<bool> UpdateLabOrder(LabOrder? model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PutAsJsonAsync($"api/laborders/{model!.Id}", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> AddProductOrder(PharmacyOrder model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/orders", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public List<ProductOrderItem> DrugOrderItems(Guid id, List<OrderCartRow> rows)
    {
        if (!rows.Any(x => x.SelectedOption == "Pharmacy"))
            return new List<ProductOrderItem>();
        var items = rows.Where(x => x.SelectedOption == "Pharmacy").Select(i => new ProductOrderItem
        {
            OrderId = id,
            ProductId = i!.Product!.Id,
            Product = i!.Product!.Item!.ProductName,
            Cost = i!.Cost,
            Quantity = i!.Quantity,

        }).ToList();
        return items;
    }

    public async Task<GridDataResponse<OrderWithData>?> GetOrderByStore(string Type, PaginationParameter parameter)
    {
        HttpResponseMessage? response = null;
        try
        {
            if (Type == "Lab")
                response = await _client.CreateClient("AppUrl").PostAsJsonAsync("api/laborders/paged", parameter);
            else
                response = await _client.CreateClient("AppUrl").PostAsJsonAsync("api/orders/paged", parameter);

            return await response.Content.ReadFromJsonAsync<GridDataResponse<OrderWithData>>();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<int> GetReceiptNo(string Type, Guid StoreID)
    {
        int receiptNbr;
        if (Type == "Lab")
            receiptNbr = await _client.CreateClient("AppUrl").GetFromJsonAsync<int>($"api/laborders/receiptno/{StoreID}");
        else
            receiptNbr = await _client.CreateClient("AppUrl").GetFromJsonAsync<int>($"api/orders/receiptno/{StoreID}");
        return receiptNbr;
    }

    public  List<LabOrderItem> LabOrderItems(Guid id, List<OrderCartRow> rows)
    {
        if (!rows.Any(x => x.SelectedOption == "Lab"))
            return new List<LabOrderItem>();

        var items = rows.Where(x => x.SelectedOption == "Lab").Select(i => new LabOrderItem
        {
            OrderId = id,
            ServiceId = i!.Service!.Id,
            ServiceName = i!.Service!.TestName,
            Cost = i!.Cost,
        }).ToList();
        return items;
    }
    public async Task GetBillQrCode(Guid id, string Type)
    {
        var code = new Converter().ConvertImageToByte(id);
        var document = new BillReceipt(code, Type);
        byte[]? content = document.Create();
        if (content != null)
            await _js.InvokeAsync<object>("exportFile", $"Receipt-{Guid.NewGuid()}.pdf", Convert.ToBase64String(content));
    }
    public async Task GetReceipt(string Type, ReportData report)
    {
        byte[]? content = null;
        try
        {
            Guid OrderID;
            if (report.ReportType == "Lab")
                OrderID = report.LabOrder!.Id;
            else
                OrderID = report.Order!.Id;

            report.ReportHeader = new ReportHeader
            {
                Store = report.Branch,
                Logo = await _client.CreateClient("AppUrl").GetByteArrayAsync("icon-512.png"),
                Title = report.ReportType
            };
            report.ReportFooter = new ReportFooter();
            report.ReportFooter.QR = new Converter().ConvertImageToByte(OrderID);
            var receipt = new ReceiptTemplate(report);
            content = await receipt.Create();
            if (content != null)
                await _js.InvokeAsync<object>("exportFile", $"Receipt-{Guid.NewGuid()}.pdf", Convert.ToBase64String(content));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }      
    }

    public ICollection<OrderReferer> GetOrderReferers(Guid id, string type, ICollection<OrderReferer> referers)
    {
        if (!referers.Any())
            return new List<OrderReferer>();

        foreach (var item in referers)
        {
            if (type == "Lab")
                item.LabOrderId = id;
            else
                item.OrderId = id;
        }
        return referers;
    }

    public async Task<LabOrder?> GetLabOrder(Guid id)
    {
        return await _client.CreateClient("AppUrl").GetFromJsonAsync<LabOrder?>($"api/laborders/{id}");
    }
    
    public async Task<LabOrder?> GetLabOrder(int receiptNumber)
    {
        return await _client.CreateClient("AppUrl").GetFromJsonAsync<LabOrder?>($"api/laborders/byreceiptno/{receiptNumber}");
    }

    public async Task<PharmacyOrder?> GetPharmacyOrder(Guid id)
    {
        return await _client.CreateClient("AppUrl").GetFromJsonAsync<PharmacyOrder?>($"api/orders/{id}");
    }
    public async Task<PharmacyOrder?> GetPharmacyOrder(int receiptNumber)
    {
        return await _client.CreateClient("AppUrl").GetFromJsonAsync<PharmacyOrder?>($"api/orders/byreceiptno/{receiptNumber}");
    }

    public async Task GetLabResults(Guid id)
    {
        ReportData report = new();
        byte[]? content = null;
        try
        {

            LabOrder? order = await GetLabOrder(id);
            Guid OrderID = id;            

            report.ReportHeader = new ReportHeader
            {
                Store = order!.Store,
                Logo = await _client.CreateClient("AppUrl").GetByteArrayAsync("icon-512.png"),
                Title = "Laboratories"
            };
            report.LabOrder = order;
            report.ReportFooter = new ReportFooter();
            report.ReportFooter.QR = new Converter().ConvertImageToByte(OrderID);
            report.ReportFooter.User = order.Diagonses!.Select(x => x.LabScientist).FirstOrDefault(new Shared.Models.Users.User());
            var receipt = new LabTemplate(report);
            content = await receipt.Create();
            if (content != null)
                await _js.InvokeAsync<object>("exportFile", $"Lab-{Guid.NewGuid()}.pdf", Convert.ToBase64String(content));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
