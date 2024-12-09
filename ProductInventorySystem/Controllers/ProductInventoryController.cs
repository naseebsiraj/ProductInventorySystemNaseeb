using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductInventorySystem.Models;
using ProductInventorySystem.Repository;
using ProductInventorySystem.ViewModel;


namespace ProductInventorySystem.Controllers
{
    //Controller for managing Product Inventory operations 
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryController : ControllerBase
    {
        // Call Product Repository
        private readonly IProductRepository _repository;

        // Dependency Injection
        public ProductInventoryController(IProductRepository repository)
        {
            _repository = repository;
        }

        // Get Product Details - ViewModel
        [HttpGet("available")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<ProductVarientViewModel>>> GetAvailableProductsViewModel()
        {
            var products = await _repository.GetAllProductsViewModel();
            return Ok(products);
        }

        // Create new Product 
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductViewModel request)
        {
            var userId = Guid.Parse("9C995A71-3BC7-4020-844E-14A9036AE0D7");
            var product = await _repository.CreateProduct(request, userId);
            return Ok(product);
        }

        // Add Stock
        [HttpPost("stock/add")]
        public async Task<ActionResult> AddStock([FromBody] StockManagementViewModel model)
        {
            var result = await _repository.AddStock(model);
            if (!result) return NotFound("Product variant not found");
            return Ok("Stock added successfully");
        }

        // Remove Stock
        [HttpPost("stock/remove")]
        public async Task<ActionResult> RemoveStock([FromBody] StockManagementViewModel model)
        {
            var result = await _repository.RemoveStock(model);
            if (!result) return BadRequest("Insufficient stock or product variant not found");
            return Ok("Stock removed successfully");
        }


    }
}
