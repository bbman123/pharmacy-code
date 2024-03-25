using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Charges;
using Shared.Models.Products;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChargesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ChargesController> _logger;

    public ChargesController(AppDbContext context, ILogger<ChargesController> logger)
    {
        _context = context;
        _logger = logger;
    }
    [HttpPost("paged")]
    public async Task<ActionResult<GridDataResponse<Charge>>> PagedCharges(PaginationParameter parameter, CancellationToken cancellationToken)
    {
        GridDataResponse<Charge> response = new();
        response!.Data = await _context.Charges.OrderByDescending(o => o.ModifiedDate).Skip(parameter.Page).Take(parameter.PageSize).ToListAsync();
        response!.TotalCount = await _context.Charges.CountAsync();
        return response;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Charge>>> GetCharges()
    {
        return await _context.Charges.OrderByDescending(x => x.CreatedDate).ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Charge?>> GetCharge(Guid id)
    {
        if (_context.Charges == null)
        {
            return NotFound();
        }
        var category = await _context.Charges.FindAsync(id);
        return category;
    }

    // PUT: api/Charge/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCharge(Guid id, Charge category)
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
            if (!ChargeExists(id))
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

    // POST: api/Charges
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    [HttpPost]
    public async Task<ActionResult<Charge>> PostCharge(Charge category)
    {
        if (_context.Charges == null)
        {
            return Problem("Entity set 'AppDbContext.Charges'  is null.");
        }
        _context.Charges.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCharge", new { id = category.Id }, category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var record = await _context.Charges.FindAsync(id);
        if (record is null)
            return NotFound();

        _context.Charges.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    public static GridDataResponse<Charge> Paginate(IQueryable<Charge> source, PaginationParameter parameters)
    {
        int totalItems = source.Count();
        int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

        List<Charge> items = new();
        items = source
                    .OrderByDescending(c => c.CreatedDate)
                    .Skip(parameters.Page)
                    .Take(parameters.PageSize)
                    .ToList();

        return new GridDataResponse<Charge>
        {
            Data = items,
            TotalCount = totalItems
        };
    }

    private bool ChargeExists(Guid id)
    {
        return (_context.Charges?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
