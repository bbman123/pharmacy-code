using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Orders;

namespace Server.Controllers;

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
	public async Task<ActionResult<GridDataResponse<Order>>> PagedCategories(PaginationParameter parameter, CancellationToken cancellationToken)
	{
		IQueryable<Order> query;
		Order[] data = new Order[0];
        data = await _context.Orders.AsNoTracking()
                                 .AsSplitQuery()
                                 .Include(x => x.Store)
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



	public static GridDataResponse<Order> Paginate(IQueryable<Order> source, PaginationParameter parameters)
	{
		int totalItems = source.Count();
		int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

		List<Order> items = new();
		items = source
					.OrderByDescending(c => c.CreatedDate)
					.Skip(parameters.Page)
					.Take(parameters.PageSize)
					.ToList();

		return new GridDataResponse<Order>
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
