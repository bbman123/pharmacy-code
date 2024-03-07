using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Products;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly ILogger<ProductsController> _logger;

	public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
	{
		_context = context;
		_logger = logger;
	}
	[HttpPost("paged")]
	public async Task<ActionResult<GridDataResponse<Product>>> PagedProducts(PaginationParameter parameter, CancellationToken cancellationToken)
	{
		IQueryable<Product> query;
		Product[] data = new Product[0];
		if (parameter.FilterId is null)
		{
            data = await _context.Products.AsNoTracking()
                                .AsSplitQuery()
                                .Include(x => x.Category)
                                .ToArrayAsync(cancellationToken);
        }		
		else
		{
            data = await _context.Products.AsNoTracking()
                                .AsSplitQuery()
                                .Include(x => x.Category)
								.Where(x => x.CategoryID == parameter.FilterId)
                                .ToArrayAsync(cancellationToken);
        }
		query = data.AsQueryable();
		var pagedResult = Paginate(query, parameter);
		return Ok(pagedResult);
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
	{
		return await _context.Products.OrderByDescending(x => x.CreatedDate).ToArrayAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Product?>> GetProduct(Guid id)
	{
		if (_context.Products == null)
		{
			return NotFound();
		}
		var Product = await _context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
		return Product;
	}

    // GET: api/Products/5
    [HttpGet("byBranch/{id}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByBranch(Guid id)
    {
        var products = await _context.Products.AsSplitQuery()
                                             .Include(b => b.Store)
                                             .Include(c => c.Category)
                                             .Where(x => x.StoreId == id && x.StocksOnHand >= 1)
                                             .OrderByDescending(x => x.ModifiedDate)
                                             .Select(p => new Product
                                             {
                                                 Id = p.Id,
                                                 Barcode = p.Barcode,
                                                 ProductName = p.ProductName,
                                                 Description = p.Description,
                                                 CategoryID = p.CategoryID,
                                                 UnitPrice = p.UnitPrice,
                                                 ReorderLevel = p.ReorderLevel,
                                                 StocksOnHand = p.StocksOnHand,
                                                 CreatedDate = p.CreatedDate,
                                                 ModifiedDate = p.ModifiedDate
                                             }).ToArrayAsync();

        if (products == null)
        {
            return NotFound();
        }

        return products;
    }

    // PUT: api/Product/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
	public async Task<IActionResult> PutProduct(Guid id, Product Product)
	{
		if (id != Product.Id)
		{
			return BadRequest();
		}

		_context.Entry(Product).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!ProductExists(id))
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

	// POST: api/Products
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

	[HttpPost]
	public async Task<ActionResult<Product>> PostProduct(Product Product)
	{
		if (_context.Products == null)
		{
			return Problem("Entity set 'AppDbContext.Products'  is null.");
		}
		_context.Products.Add(Product);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetProduct", new { id = Product.Id }, Product);
	}



	public static GridDataResponse<Product> Paginate(IQueryable<Product> source, PaginationParameter parameters)
	{
		int totalItems = source.Count();
		int totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

		List<Product> items = new();
		items = source
					.OrderByDescending(c => c.CreatedDate)
					.Skip(parameters.Page)
					.Take(parameters.PageSize)
					.ToList();

		return new GridDataResponse<Product>
		{
			Data = items,
			TotalCount = totalItems
		};
	}

	private bool ProductExists(Guid id)
	{
		return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}
