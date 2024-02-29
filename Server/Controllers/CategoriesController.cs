using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Products;
using System.Threading;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

		public CategoriesController(AppDbContext context, ILogger<CategoriesController> logger)
		{
			_context = context;
			_logger = logger;
		}
		[HttpPost("paged")]
		public async Task<ActionResult<GridDataResponse<Category>>> PagedCategories(PaginationParameter parameter, CancellationToken cancellationToken)
		{
			IQueryable<Category> query;
			Category[] data = new Category[0];
			data = await _context.Categories.AsNoTracking().ToArrayAsync(cancellationToken);
			query = string.IsNullOrEmpty(parameter.SearchTerm) == true ? data.AsQueryable() : data.Where(x => x.ToString()!.Contains(parameter!.SearchTerm!, StringComparison.InvariantCultureIgnoreCase)).AsQueryable();
			var pagedResult = Paginate(query, parameter);
			return Ok(pagedResult);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
		{
			return await _context.Categories.OrderByDescending(x => x.CreatedDate).ToArrayAsync();
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<Category?>> GetCategory(Guid id)
		{
			if (_context.Categories == null)
			{
				return NotFound();
			}
			var category =  await _context.Categories.FindAsync(id);
			return category;
		}

		// PUT: api/Category/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCategory(Guid id, Category category)
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
				if (!CategoryExists(id))
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

		// POST: api/Categorys
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

		[HttpPost]
		public async Task<ActionResult<Category>> PostCategory(Category category)
		{
			if (_context.Categories == null)
			{
				return Problem("Entity set 'AppDbContext.Categorys'  is null.");
			}
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();			

			return CreatedAtAction("GetCategory", new { id = category.Id }, category);
		}



		public static GridDataResponse<Category> Paginate(IQueryable<Category> source, PaginationParameter parameters)
		{
			int totalItems = source.Count();
			int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

			List<Category> items = new();
			items = source
						.OrderByDescending(c => c.CreatedDate)
						.Skip(parameters.Page)
						.Take(parameters.PageSize)
						.ToList();

			return new GridDataResponse<Category>
			{
				Data = items,
				TotalCount = totalItems
			};
		}

		private bool CategoryExists(Guid id)
		{
			return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
