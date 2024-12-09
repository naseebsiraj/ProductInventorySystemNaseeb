using ProductInventorySystem.Models;
using ProductInventorySystem.ViewModel;

namespace ProductInventorySystem.Repository
{
    public interface IProductRepository
    {

        // Create New Products
        public Task<Product> CreateProduct(CreateProductViewModel request, Guid userId);

        // Get All Available Products Details without ViewModel
        //public Task<IEnumerable<Product>> GetAllProducts();

        // Get all Available Products- ViewModel
        public Task<IEnumerable<ProductVarientViewModel>> GetAllProductsViewModel();

        // Add Stock +(Purchase)
        public Task<bool> AddStock(StockManagementViewModel model);

        // Remove stock -(Sale)
        public Task<bool> RemoveStock(StockManagementViewModel model);


    }
}
