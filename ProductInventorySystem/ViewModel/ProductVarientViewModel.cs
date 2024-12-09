namespace ProductInventorySystem.ViewModel
{
    public class ProductVarientViewModel
    {
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string VarientName { get; set; } = null!;
        public string OptionValue { get; set; } = null!;
        public decimal Stock { get; set; }
    }
}
