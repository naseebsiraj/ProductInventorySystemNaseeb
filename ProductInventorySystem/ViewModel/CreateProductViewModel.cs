namespace ProductInventorySystem.ViewModel
{
    public class CreateProductViewModel
    {
        // Product name from Request
        public string Name { get; set; }

        // Varients list with option (Size/color)
        public List<VarientViewModel> Variants { get; set; }
    }
}
