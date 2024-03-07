using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Enums;
using Shared.Helpers;
using Shared.Models.Products;
using Shared.Models.Users;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReferersController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReferersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("paged")]
    public async Task<ActionResult<GridDataResponse<Referer>>> PagedReferers(PaginationParameter parameter, CancellationToken cancellationToken)
    {
        IQueryable<Referer> query;
        Referer[] data = [];
        data = await _context.Referers.AsNoTracking().ToArrayAsync(cancellationToken);
        query = string.IsNullOrEmpty(parameter.SearchTerm) == true ? data.AsQueryable() : data.Where(x => x.ToString()!.Contains(parameter!.SearchTerm!, StringComparison.InvariantCultureIgnoreCase)).AsQueryable();
        var pagedResult = Paginate(query, parameter);
        return Ok(pagedResult);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Referer>>> GetReferers()
    {
        return await _context.Referers.OrderByDescending(x => x.CreatedDate).ToArrayAsync();
    }

    [HttpGet("byType/{type}")]
    public async Task<ActionResult<IEnumerable<Referer>>> GetReferers(RefererType type)
    {
        return await _context.Referers.AsNoTracking()
                                      .Where(x => x.Type == type)
                                      .OrderBy(x => x.RefererName)
                                      .ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Referer?>> GetReferer(Guid id)
    {
        if (_context.Referers == null)
        {
            return NotFound();
        }
        var category = await _context.Referers.FindAsync(id);
        return category;
    }

    // PUT: api/Referer/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReferer(Guid id, Referer category)
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
            if (!RefererExists(id))
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

    // POST: api/Referers
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    [HttpPost]
    public async Task<ActionResult<Referer>> PostReferer(Referer category)
    {
        if (_context.Referers == null)
        {
            return Problem("Entity set 'AppDbContext.Referers'  is null.");
        }
        _context.Referers.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetReferer", new { id = category.Id }, category);
    }



    public static GridDataResponse<Referer> Paginate(IQueryable<Referer> source, PaginationParameter parameters)
    {
        int totalItems = source.Count();
        int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

        List<Referer> items = new();
        items = source
                    .OrderByDescending(c => c.CreatedDate)
                    .Skip(parameters.Page)
                    .Take(parameters.PageSize)
                    .ToList();

        return new GridDataResponse<Referer>
        {
            Data = items,
            TotalCount = totalItems
        };
    }

    private bool RefererExists(Guid id)
    {
        return (_context.Referers?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
