using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Enums;
using Shared.Helpers;
using Shared.Models.Labs;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LabTestsController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly ILogger<CategoriesController> _logger;

	public LabTestsController(AppDbContext context, ILogger<CategoriesController> logger)
	{
		_context = context;
		_logger = logger;
	}
	[HttpPost("paged")]
	public async Task<ActionResult<GridDataResponse<LabTest>>> PagedCategories(PaginationParameter parameter, CancellationToken cancellationToken)
	{
		IQueryable<LabTest> query;
		LabTest[] data = new LabTest[0];
		data = await _context.LabTests.AsNoTracking().ToArrayAsync(cancellationToken);
		query = string.IsNullOrEmpty(parameter.SearchTerm) == true ? data.AsQueryable() : data.Where(x => x.ToString()!.Contains(parameter!.SearchTerm!, StringComparison.InvariantCultureIgnoreCase)).AsQueryable();
		var pagedResult = Paginate(query, parameter);
		return Ok(pagedResult);
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<LabTest>>> GetServices()
	{
		return await _context.LabTests.OrderByDescending(x => x.CreatedDate).ToArrayAsync();
	}
    
    [HttpGet("type/{type}")]
	public async Task<ActionResult<IEnumerable<LabTest>>> GetServices(ServiceType type)
	{
		return await _context.LabTests.Where(x => x.Type == type).OrderByDescending(x => x.CreatedDate).ToArrayAsync();
	}



	[HttpGet("{id}")]
	public async Task<ActionResult<LabTest?>> GetLabTest(Guid id)
	{
		if (_context.LabTests == null)
		{
			return NotFound();
		}
		var category = await _context.LabTests.FindAsync(id);
		return category;
	}

	// PUT: api/LabTests/5
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPut("{id}")]
	public async Task<IActionResult> PutLabTest(Guid id, LabTest category)
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
			if (!LabTestExists(id))
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

	// POST: api/LabTests
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

	[HttpPost]
	public async Task<ActionResult<LabTest>> PostLabTest(LabTest category)
	{
		if (_context.LabTests == null)
		{
			return Problem("Entity set 'AppDbContext.LabTests'  is null.");
		}
		_context.LabTests.Add(category);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetLabTest", new { id = category.Id }, category);
	}



	public static GridDataResponse<LabTest> Paginate(IQueryable<LabTest> source, PaginationParameter parameters)
	{
		int totalItems = source.Count();
		int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

		List<LabTest> items = new();
		items = source
					.OrderByDescending(c => c.CreatedDate)
					.Skip(parameters.Page)
					.Take(parameters.PageSize)
					.ToList();

		return new GridDataResponse<LabTest>
		{
			Data = items,
			TotalCount = totalItems
		};
	}

	private bool LabTestExists(Guid id)
	{
		return (_context.LabTests?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}
