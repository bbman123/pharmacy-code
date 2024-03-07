using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Labs;
using Shared.Models.Products;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiagnosisController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<DiagnosisController> _logger;

    public DiagnosisController(AppDbContext context, ILogger<DiagnosisController> logger)
    {
        _context = context;
        _logger = logger;
    }
    [HttpPost("paged")]
    public async Task<ActionResult<GridDataResponse<LabDiagnose>>> PagedDiagnosis(PaginationParameter parameter, CancellationToken cancellationToken)
    {
        IQueryable<LabDiagnose> query;
        LabDiagnose[] data = [];
        data = await _context.LabDiagonses.AsNoTracking()
                                          .AsSplitQuery()
                                          .Include(x => x.Order)
                                          .ToArrayAsync(cancellationToken);
        query = string.IsNullOrEmpty(parameter.SearchTerm) == true ? data.AsQueryable() : data.Where(x => x.ToString()!.Contains(parameter!.SearchTerm!, StringComparison.InvariantCultureIgnoreCase)).AsQueryable();
        var pagedResult = Paginate(query, parameter);
        return Ok(pagedResult);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabDiagnose>>> GetDiagnosis()
    {
        return await _context.LabDiagonses.AsNoTracking()
                                          .AsSplitQuery()                                          
                                          .Include(x => x.Order)
                                          .Include(x => x.Test)
                                          .OrderByDescending(x => x.Order!.CreatedDate)
                                          .ToArrayAsync();
    }
    
    [HttpGet("investigations/{id}")]
    public async Task<ActionResult<IEnumerable<LabDiagnose>>> GetDiagnosis(Guid id)
    {
        return await _context.LabDiagonses.AsNoTracking()
                                          .AsSplitQuery()                                          
                                          .Include(x => x.Order)
                                          .Include(x => x.Test)
                                          .Where(x => x.LabOrderId == id)                                          
                                          .ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LabDiagnose?>> GetDiagnose(Guid id)
    {
        if (_context.LabDiagonses == null)
        {
            return NotFound();
        }
        var diagonse = await _context.LabDiagonses.AsNoTracking()
                                                  .AsSplitQuery()
                                                  .Include(x => x.Order)
                                                  .Include(x => x.Test)
                                                  .SingleOrDefaultAsync(s => s.Id == id);
        return diagonse;
    }

    // PUT: api/LabDiagonse/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(Guid id, LabDiagnose diagonse)
    {
        if (id != diagonse.Id)
        {
            return BadRequest();
        }

        _context.Entry(diagonse).State = EntityState.Modified;

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
    public async Task<ActionResult<LabDiagnose>> PostCategory(LabDiagnose diagonse)
    {
        if (_context.LabDiagonses == null)
        {
            return Problem("Entity set 'AppDbContext.LabDiagnoses'  is null.");
        }
        _context.LabDiagonses.Add(diagonse);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDiagnose", new { id = diagonse.Id }, diagonse);
    }



    public static GridDataResponse<LabDiagnose> Paginate(IQueryable<LabDiagnose> source, PaginationParameter parameters)
    {
        int totalItems = source.Count();
        int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

        List<LabDiagnose> items = new();
        items = source
                    .OrderByDescending(c => c.Order!.CreatedDate)
                    .Skip(parameters.Page)
                    .Take(parameters.PageSize)
                    .ToList();

        return new GridDataResponse<LabDiagnose>
        {
            Data = items,
            TotalCount = totalItems
        };
    }

    private bool CategoryExists(Guid id)
    {
        return (_context.LabDiagonses?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}

