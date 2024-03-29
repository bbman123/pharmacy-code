﻿using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Labs;
using Shared.Models.Orders;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LabOrdersController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly ILogger<LabOrdersController> _logger;

	public LabOrdersController(AppDbContext context, ILogger<LabOrdersController> logger)
	{
		_context = context;
		_logger = logger;
	}
	[HttpPost("paged")]
	public async Task<ActionResult<GridDataResponse<OrderWithData>>> PagedCategories(PaginationParameter parameter, CancellationToken cancellationToken)
	{
        GridDataResponse<OrderWithData>? response = new();
        response!.Data = await _context.LabOrders.AsNoTracking()
                                       .AsSplitQuery()
                                       .Include(x => x.Store)
                                       .Include(x => x.Items)
                                       .Where(store => store.StoreId == parameter.FilterId)
                                       .OrderByDescending(c => c.CreatedDate)
                                        .Skip(parameter.Page)
                                        .Take(parameter.PageSize)
                                        .Select(x => new OrderWithData
                                        {
                                            Id = x.Id,
                                            Date = x.OrderDate,
                                            StoreName = x.Store!.BranchName,
                                            OrderType = "Lab",
                                            ReceiptNo = x.ReceiptNo,
                                            TotalAmount = x.Total,
                                            Status = x.Status,
                                            CreatedDate = x.CreatedDate,
                                            ModifiedDate = x.ModifiedDate
                                        }).ToListAsync();
        response!.TotalCount = await _context.LabOrders.Where(store => store.StoreId == parameter.FilterId).CountAsync();
        return response;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<LabOrder>>> GetCategories()
	{
		return await _context.LabOrders.OrderByDescending(x => x.CreatedDate).ToArrayAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<LabOrder?>> GetLabOrder(Guid id)
	{
		if (_context.LabOrders == null)
		{
			return NotFound();
		}
        var order = await _context.LabOrders.AsNoTracking()
                                      .AsSplitQuery()
                                      .Include(s => s!.Customer!)
                                      .Include(s => s!.User!)
                                      .Include(s => s!.ConsultedBy)
                                      .Include(s => s!.Store!)
                                      .Include(s => s!.Items!)
                                      .Include(s => s!.Diagonses!)
                                      .ThenInclude(s => s!.LabScientist)
                                      .SingleOrDefaultAsync(x => x.Id == id);
		return order;
	}
    
    [HttpGet("byreceiptno/{rno}")]
	public async Task<ActionResult<LabOrder?>> GetLabOrder(int rno)
	{
		if (_context.LabOrders == null)
		{
			return NotFound();
		}
        var order = await _context.LabOrders.AsNoTracking()
                                      .AsSplitQuery()
                                      .Include(s => s!.Customer!)
                                      .Include(s => s!.User!)
                                      .Include(s => s!.ConsultedBy)
                                      .Include(s => s!.Store!)
                                      .Include(s => s!.Items!)
                                      .Include(s => s!.Diagonses!)
                                      .ThenInclude(s => s!.LabScientist)
                                      .SingleOrDefaultAsync(x => x.ReceiptNo == rno);
		return order;
	}

    [HttpGet("receiptno/{id}")]
    public async Task<ActionResult<int>> GetReceiptNo(Guid id)
    {
        var result = await _context.LabOrders.Where(b => b.StoreId == id)
                                          .Select(i => i.ReceiptNo)
                                          .CountAsync();
        result += 1;

        return result;
    }

    // PUT: api/LabOrders/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
	public async Task<IActionResult> PutLabOrder(Guid id, LabOrder order)
	{
		if (id != order.Id)
		{
			return BadRequest();
		}

		_context.Entry(order).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!LabOrderExists(id))
			{
				return NotFound();
			}
			else
			{
				throw;
			}
		}

		return NoContent();
	}

	// POST: api/LabOrders
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

	[HttpPost]
	public async Task<ActionResult<LabOrder>> PostLabOrder(LabOrder order)
	{
		if (_context.LabOrders == null)
		{
			return Problem("Entity set 'AppDbContext.LabOrders'  is null.");
		}
		_context.LabOrders.Add(order);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetLabOrder", new { id = order.Id }, order);
	}

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var record = await _context.LabOrders.FindAsync(id);
        if (record is null)
            return NotFound();

        _context.LabOrders.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    public static GridDataResponse<OrderWithData> Paginate(IQueryable<LabOrder> source, PaginationParameter parameters)
	{
		int totalItems = source.Count();
		int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

		List<OrderWithData> items = new();
		items = source
					.OrderByDescending(c => c.CreatedDate)
					.Skip(parameters.Page)
					.Take(parameters.PageSize)
                    .Select(x => new OrderWithData
                    {
                        Id = x.Id,
                        Date = x.OrderDate,
                        StoreName = x.Store!.BranchName,
                        OrderType = "Lab",
                        ReceiptNo = x.ReceiptNo,
                        TotalAmount = x.Total,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate,
                        ModifiedDate = x.ModifiedDate
                    }).ToList();

        return new GridDataResponse<OrderWithData>
		{
			Data = items,
			TotalCount = totalItems
		};
	}

	private bool LabOrderExists(Guid id)
	{
		return (_context.LabOrders?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}
