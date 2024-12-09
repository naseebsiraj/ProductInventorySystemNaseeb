using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductInventorySystem.Models;

public partial class ProductVarient
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public string VarientName { get; set; } = null!;

    [Required]
    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<VariantOption> VariantOptions { get; set; } = new List<VariantOption>();
}
