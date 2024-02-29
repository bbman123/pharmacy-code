using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Auth;

public class LoginResponse
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Token { get; set; }
}
