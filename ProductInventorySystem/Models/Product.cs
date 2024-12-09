using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductInventorySystem.Models;

public partial class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string ProductCode { get; set; } = null!;

    [Required]
    public string ProductName { get; set; } = null!;

    public byte[]? ProductImage { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset UpdatedDate { get; set; }

    public Guid CreatedUserId { get; set; }

    public bool IsFavorite { get; set; }

    public bool Active { get; set; }

    public string? Hsncode { get; set; }

    public decimal TotalStock { get; set; }


    // Navigation Properties
    public virtual User CreatedUser { get; set; } = null!;

    public virtual ICollection<ProductVarient> ProductVarients { get; set; } = new List<ProductVarient>();
}
