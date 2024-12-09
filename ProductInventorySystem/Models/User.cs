using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductInventorySystem.Models;

public partial class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    [Required]
    public string Password { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
