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
    public async Task<ActionResult<GridDataResponse<Referer>>> PagedReferers(PaginationParameter parameters, CancellationToken cancellationToken)
    {
        var response = new GridDataResponse<Referer>()
        {
            Data = await _context.Referers.AsNoTracking()
                                          .AsSplitQuery()
                                          .Include(o => o.OrderReferers)
                                          .OrderByDescending(c => c.ModifiedDate)
                                          .Skip(parameters.Page)
                                          .Take(parameters.PageSize)
                                          .ToListAsync(cancellationToken),
            TotalCount = await _context.Referers.CountAsync()

        };
        return response;
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
                                      .AsSplitQuery()
                                      .Include(o => o.OrderReferers)
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
        var referrer = await _context.Referers.AsNoTracking()
                                              .AsSplitQuery()
                                              .Include(o => o.OrderReferers)
                                              .ThenInclude(o => o.LabOrder)
                                              .Include(o => o.OrderReferers)
                                              .ThenInclude(o => o.PharmacyOrder)
                                              .SingleOrDefaultAsync(x => x.Id == id);
        return referrer;
    }

    // PUT: api/Referer/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReferer(Guid id, Referer referrer)
    {
        if (id != referrer.Id)
        {
            return BadRequest();
        }

        _context.Entry(referrer).State = EntityState.Modified;

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
    public async Task<ActionResult<Referer>> PostReferer(Referer referrer)
    {
        if (_context.Referers == null)
        {
            return Problem("Entity set 'AppDbContext.Referers'  is null.");
        }
        _context.Referers.Add(referrer);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetReferer", new { id = referrer.Id }, referrer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var record = await _context.Referers.FindAsync(id);
        if (record is null)
            return NotFound();

        _context.Referers.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
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
