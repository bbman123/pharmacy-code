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
	public async Task<ActionResult<GridDataResponse<OrderWithData>>> PagedOrders(PaginationParameter parameter, CancellationToken cancellationToken)
	{
		IQueryable<Order> query;
        Order[] data = [];
        data = await _context.Orders.AsNoTracking()
                                    .AsSplitQuery()
                                    .Include(x => x.Store)
                                    .Include(x => x.ProductOrders)
                                    .Where(store => store.StoreId == parameter.FilterId)
                                    .ToArrayAsync(cancellationToken);
        query = data.AsQueryable();
        var pagedResult = Paginate(query, parameter);
		return Ok(pagedResult);
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Order>>> GetCategories()
	{
		return await _context.Orders.OrderByDescending(x => x.CreatedDate).ToArrayAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Order?>> GetOrder(Guid id)
	{
		if (_context.Orders == null)
		{
			return NotFound();
		}
		var category = await _context.Orders.FindAsync(id);
		return category;
	}

	// PUT: api/Orders/5
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPut("{id}")]
	public async Task<IActionResult> PutOrder(Guid id, Order category)
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
	public async Task<ActionResult<Order>> PostOrder(Order category)
	{
		if (_context.Orders == null)
		{
			return Problem("Entity set 'AppDbContext.Orders'  is null.");
		}
		_context.Orders.Add(category);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetOrder", new { id = category.Id }, category);
	}



	public static GridDataResponse<OrderWithData> Paginate(IQueryable<Order> source, PaginationParameter parameters)
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
