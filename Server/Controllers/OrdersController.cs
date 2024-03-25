using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Orders;

namespace Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly ILogger<OrdersController> _logger;

	public OrdersController(AppDbContext context, ILogger<OrdersController> logger)
	{
		_context = context;
		_logger = logger;
	}
	[HttpPost("paged")]
	public async Task<ActionResult<GridDataResponse<OrderWithData>?>> PagedOrders(PaginationParameter parameter, CancellationToken cancellationToken)
	{
        GridDataResponse<OrderWithData>? response = new();

        response!.Data = await _context.Orders.AsNoTracking()
                                            .AsSplitQuery()
                                            .Include(x => x.Store)
                                            .Include(x => x.ProductOrders)
                                            .Where(store => store.StoreId == parameter.FilterId)
                                            .OrderByDescending(x => x.ModifiedDate)
                                            .Skip(parameter.Page)
                                            .Take(parameter.PageSize)
                                            .Select(x => new OrderWithData
                                            {
                                                Id = x.Id,
                                                Date = x.OrderDate,
                                                StoreName = x.Store!.BranchName,
                                                OrderType = "Pharmacy",
                                                ReceiptNo = x.ReceiptNo,
                                                TotalAmount = x.TotalAmount,
                                                CreatedDate = x.CreatedDate,
                                                ModifiedDate = x.ModifiedDate
                                            }).ToListAsync(cancellationToken);
        response!.TotalCount = await _context.Orders.Where(store => store.StoreId == parameter.FilterId).CountAsync(cancellationToken);
        return response;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<PharmacyOrder>>> GetCategories()
	{
		return await _context.Orders.OrderByDescending(x => x.CreatedDate).ToArrayAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<PharmacyOrder?>> GetOrder(Guid id)
	{
		if (_context.Orders == null)
		{
			return NotFound();
		}
		var category = await _context.Orders.AsNoTracking()
                                      .AsSplitQuery()
                                      .Include(x => x.ProductOrders)
                                      .Include(x => x.ConsultedBy)
                                      .Include(x => x.User)
                                      .Include(x => x.Store)
                                      .Include(x => x.Referers)
                                      .SingleOrDefaultAsync(x => x.Id == id);
		return category;
	}
    
    [HttpGet("byreceiptno/{rno}")]
	public async Task<ActionResult<PharmacyOrder?>> GetOrderByReceiptNo(int rno)
	{
		if (_context.Orders == null)
		{
			return NotFound();
		}
		var category = await _context.Orders.AsNoTracking()
                                      .AsSplitQuery()
                                      .Include(x => x.ProductOrders)
                                      .Include(x => x.ConsultedBy)
                                      .Include(x => x.User)
                                      .Include(x => x.Store)
                                      .Include(x => x.Referers)
                                      .SingleOrDefaultAsync(x => x.ReceiptNo == rno);
		return category;
	}

	// PUT: api/Orders/5
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPut("{id}")]
	public async Task<IActionResult> PutOrder(Guid id, PharmacyOrder category)
	{
		if (id != category.Id)
		{
			return BadRequest();
		}

		_context.Entry(category).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!OrderExists(id))
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
    
    [HttpGet("receiptno/{id}")]
    public async Task<ActionResult<int>> GetReceiptNo(Guid id)
    {
        var result = await _context.Orders.Where(b => b.StoreId == id)
                                          .Select(i => i.ReceiptNo)
                                          .CountAsync();
        result += 1;

        return result;
    }

    // POST: api/Orders
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
	public async Task<ActionResult<PharmacyOrder>> PostOrder(PharmacyOrder category)
	{
		if (_context.Orders == null)
		{
			return Problem("Entity set 'AppDbContext.Orders'  is null.");
		}
		_context.Orders.Add(category);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetOrder", new { id = category.Id }, category);
	}

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var record = await _context.Orders.FindAsync(id);
        if (record is null)
            return NotFound();

        _context.Orders.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    public static GridDataResponse<OrderWithData> Paginate(IQueryable<PharmacyOrder> source, PaginationParameter parameters)
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
                        OrderType = "Pharmacy",
                        ReceiptNo = x.ReceiptNo,
                        TotalAmount = x.TotalAmount,
                        CreatedDate = x.CreatedDate,
                        ModifiedDate = x.ModifiedDate
                    }).ToList();

		return new GridDataResponse<OrderWithData>
		{
			Data = items,
			TotalCount = totalItems
		};
	}

	private bool OrderExists(Guid id)
	{
		return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}
