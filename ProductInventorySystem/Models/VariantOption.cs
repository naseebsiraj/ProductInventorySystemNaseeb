using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductInventorySystem.Models;

public partial class VariantOption
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid VarientId { get; set; }

    [Required]
    public string OptionValue { get; set; } = null!;

    public decimal Stock { get; set; }

    public virtual ProductVarient Varient { get; set; } = null!;
}
