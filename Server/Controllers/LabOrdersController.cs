using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Labs;

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
	public async Task<ActionResult<GridDataResponse<LabOrder>>> PagedCategories(PaginationParameter parameter, CancellationToken cancellationToken)
	{
		IQueryable<LabOrder> query;
		LabOrder[] data = [];
		data = await _context.LabOrders.AsNoTracking()
                                 .AsSplitQuery()
                                 .Include(x => x.Store)
                                 .Where(store => store.StoreId == parameter.FilterId)
                                 .ToArrayAsync(cancellationToken);
		query = data.AsQueryable();
        var pagedResult = Paginate(query, parameter);
		return Ok(pagedResult);
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
		var order = await _context.LabOrders.FindAsync(id);
		return order;
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



	public static GridDataResponse<LabOrder> Paginate(IQueryable<LabOrder> source, PaginationParameter parameters)
	{
		int totalItems = source.Count();
		int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

		List<LabOrder> items = new();
		items = source
					.OrderByDescending(c => c.CreatedDate)
					.Skip(parameters.Page)
					.Take(parameters.PageSize)
					.ToList();

		return new GridDataResponse<LabOrder>
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
