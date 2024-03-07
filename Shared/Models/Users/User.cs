using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;
using Shared.Models.Company;


namespace Shared.Models.Users;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? StoreId { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, ErrorMessage = "Must be atleast 8 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string? HashedPassword { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsNew { get; set; } = true;
    public UserRole Role { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
    [ForeignKey(nameof(StoreId))]
    public virtual Store? Store { get; set; }
    public override string ToString() => $"{FirstName} {LastName}";
}
