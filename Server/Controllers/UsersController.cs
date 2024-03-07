using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Context;
using Shared.Enums;
using Shared.Helpers;
using Shared.Models.Auth;
using Shared.Models.Products;
using Shared.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private IConfiguration Configuration { get; }
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        Configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }       
    
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<User>>> GetActiveUsers()
    {
        return await _context.Users.Where(x => x.IsActive).OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }          
    
    [HttpGet("byStore/{StoreID}")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersByStore(Guid StoreID)
    {
        return await _context.Users.Where(x => x.StoreId == StoreID).ToListAsync();
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<IEnumerable<User>>> GetUserByUsername(string username)
    {
        return await _context.Users.Where(x => EF.Functions.ILike(x.Username!, $"%{username}%")).ToListAsync();
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
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
    

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'AppDbContext.Users'  is null.");
        }
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }
    

    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponse>?> Login(LoginModel model)
    {
        if (_context.Users is null)
        {
            return NotFound();
        }
        string hashedPassword = Security.Encrypt(model.Password);
        var user = await _context.Users.AsNoTracking()
                                       .AsSplitQuery().Include(x => x.Store)
                                       .Where(u => u.Username == model.Username && u.HashedPassword == hashedPassword && u.IsActive == true)
                                       .SingleOrDefaultAsync();

        if (user is null)
        {
            return BadRequest();
        }

        var claim = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user!.Username!),
            new Claim(ClaimTypes.Role, user!.Role!.ToString()!)
        };

        var token = new JwtSecurityToken(
            null,
            null,
            claim,
            expires: user.Role == UserRole.Admin ? DateTime.Now.AddDays(180) : DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["App:Key"]!)),
            SecurityAlgorithms.HmacSha512Signature));

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        var result = new LoginResponse
        {
            Id = user.Id,
            Username = user.Username,
            Token = jwt,            
            StoreId = user.Role == UserRole.Admin ? null : user.StoreId,
            Role = user!.Role.ToString(),
            IsActive = user.IsActive
        };
        return result;
    }

    private bool UserExists(Guid id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
