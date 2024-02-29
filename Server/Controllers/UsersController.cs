using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Context;
using Shared.Helpers;
using Shared.Models.Auth;
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

    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponse>?> Login(LoginModel model)
    {
        string hashedPassword = Security.Encrypt(model.Password);
        var user = await _context.Users.AsNoTracking().Where(u => u.Username == model.Username
                                    && u.HashedPassword == hashedPassword
                                    && u.IsActive == true)                                   
                                   .FirstOrDefaultAsync();

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
            expires: DateTime.Now.AddDays(180),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["App:Key"]!)),
            SecurityAlgorithms.HmacSha512Signature));

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        var result = new LoginResponse
        {
            Id = user.Id,
            Username = user.Username,
            Token = jwt,            
            Role = user!.Role,
            IsActive = user.IsActive
        };
        return result;
    }
}
